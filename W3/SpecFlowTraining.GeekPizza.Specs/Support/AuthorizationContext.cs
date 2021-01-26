using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpecFlowTraining.GeekPizza.Specs.Support
{
    public class AuthorizationContext
    {
        public string CurrentUser { get; set; }

        public string AssertLoggedInUser()
        {
            Assert.IsNotNull(CurrentUser, "There should be a logged in user. Set AuthorizationContext.CurrentUser after successful login!");
            return CurrentUser;
        }
    }
}
