using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public class ParallelRuleValidator : IValidator
    {
        private readonly bool _earlyExit;
        private readonly IEnumerable<IValidationRule> _rules;

        public ParallelRuleValidator(bool earlyExit, params IValidationRule[] rules)
        {
            _earlyExit = earlyExit;
            _rules = rules ?? Enumerable.Empty<IValidationRule>();
        }

        public IValidationResult Validate(IExcelLoader excelLoader, IDictionary<string, object> context)
        {
            if (excelLoader == null) throw new ArgumentNullException("excelLoader");

            var messages = new ConcurrentBag<IValidationMessage>();
            var result = new ValidationResult {ValidationTarget = excelLoader, Messages = messages };
            var contextDic = context ?? new Dictionary<string, object>();

            try
            {
                using (var cts = new CancellationTokenSource())
                {
                    _rules.AsParallel()
                          .WithCancellation(cts.Token)
                          .ForAll(r =>
                          {
                              var ruleResult = r.Check(excelLoader, contextDic);
                              foreach (var msg in ruleResult.Messages)
                              {
                                  messages.Add(msg);
                              }
                              if (_earlyExit && !ruleResult.ShouldContinue)
                              {
                                  cts.Cancel();
                              }
                          });
                }
            }
            catch (AggregateException ex)
            {
                var errors = ex.Flatten()
                               .InnerExceptions
                               .Select(x => x.ToString().ToValidationError());
                foreach (var error in errors)
                {
                    messages.Add(error);
                }
            }
            catch (OperationCanceledException)
            {
                // Intentionally swallow this exception.
            }

            catch (Exception ex)
            {
                var error = ex.ToString().ToValidationError();
                messages.Add(error);
            }

            return result;
        }
    }
}