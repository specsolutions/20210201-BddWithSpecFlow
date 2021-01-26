using SpecFlowTraining.GeekPizza.Web.DataAccess;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.Support
{
    [Binding]
    public class DatabaseHooks
    {
        [BeforeScenario(Order = 1)]
        public void ResetDatabaseToBaseline()
        {
            ClearDatabase();

            DomainDefaults.AddDefaultUsers();
            DomainDefaults.AddDefaultPizzas();
        }

        private static void ClearDatabase()
        {
            var db = new DataContext();
            db.TruncateTables();
        }
    }
}
