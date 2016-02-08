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

            var result = new ValidationResult {ValidationTarget = excelLoader, Messages = new ConcurrentValidationMessageCollection() };
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
                                  result.Messages.Add(msg);
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
                result.Messages.AddRange(errors);
            }
            catch (OperationCanceledException)
            {
                // Intentionally swallow this exception.
            }
            catch (Exception ex)
            {
                var error = ex.ToString().ToValidationError();
                result.Messages.Add(error);
            }

            return result;
        }
    }
}