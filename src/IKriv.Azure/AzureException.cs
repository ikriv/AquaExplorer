using System;
using System.Net;

namespace IKriv.Azure
{
    public class AzureException : ApplicationException
    {
        public AzureException(string message, Exception innerException)
            :
            base(message, innerException)
        { }

        public static T Catch<T>(string description, Func<T> action)
        {
            try
            {
                return action();
            }
            catch (AzureException)
            {
                    throw;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    throw new AzureException(String.Format("Error {0}. Storage account does not exist.", description), ex);
                }
                else
                {
                    throw new AzureException(String.Format("Error {0}. {1}", description, ex.Message), ex);
                }
            }
            catch (Exception ex)
            {
                throw new AzureException(String.Format("Error {0}. {1}", description, ex.Message), ex);
            }
        }

        public static void Catch(string description, Action action)
        {
            Catch(description, ()=>{action(); return 0; });
        }
    }
}
