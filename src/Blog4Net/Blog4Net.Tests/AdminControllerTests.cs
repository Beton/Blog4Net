using System.Web.Mvc;
using Blog4Net.Web.Controllers;
using Blog4Net.Web.Models;
using Blog4Net.Web.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace Blog4Net.Tests
{
    [TestFixture]
    public class AdminControllerTests
    {
        private AdminController sut;

        private IAuthenticationService mockedAuthenticationService;

        [SetUp]
        public void SetUp()
        {
            mockedAuthenticationService = MockRepository.GenerateMock<IAuthenticationService>();

            sut = new AdminController(mockedAuthenticationService);
        }
       
        [Test]
        public void Should_redirect_authenticated_users_to_Admin_Area()
        {
            mockedAuthenticationService.Stub(s => s.IsLogged).Return(true);

            var actual = sut.Login("/admin/manage");

            Assert.IsInstanceOf<RedirectToRouteResult>(actual);
            Assert.AreEqual("Manage", ((RedirectToRouteResult)actual).RouteValues["action"]);
        }

        [Test]
        public void Should_return_Login_view_if_Admin_is_not_logged_In()
        {
            mockedAuthenticationService.Stub(s => s.IsLogged).Return(false);

            var actual = sut.Login("/returnUrl");

            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.AreEqual("/returnUrl", ((ViewResult)actual).ViewBag.ReturnUrl);
        }
        
        [Test]
        public void Should_redirect_to_Admin_area_if_credentials_are_correct()
        {
            var loginModel = new LoginModel
            {
                Username = "correct_username",
                Password = "correct_password"
            };

            mockedAuthenticationService.Stub(s => s.LogOn(loginModel.Username, loginModel.Password)).Return(true);            

            var actual = sut.Login(loginModel, "/");
            
            Assert.IsInstanceOf<RedirectToRouteResult>(actual);
            Assert.AreEqual("Manage", ((RedirectToRouteResult)actual).RouteValues["action"]);            
        }

        [Test]
        public void Should_return_Login_page_if_credentials_are_incorrect()
        {
            var loginModel = new LoginModel
            {
                Username = "invalid_username",
                Password = "invalid_passwrod"
            };

            mockedAuthenticationService.Stub(s => s.LogOn(loginModel.Username, loginModel.Password)).Return(false);            

            var actual = sut.Login(loginModel, "/");
            var modelStateErrors = sut.ModelState[""].Errors;

            Assert.IsInstanceOf<ActionResult>(actual);
            Assert.IsTrue(modelStateErrors.Count > 0);
            Assert.AreEqual("The user name or password provided is incorrect.", modelStateErrors[0].ErrorMessage);            
        }   

        [Test]
        public void Login_Post_Action_Should_check_for_validation_errors()
        {
            var loginModel = new LoginModel
                {
                    Username = "missing_user_name",
                    Password = "passwrod"
                };

            sut.ModelState.AddModelError("UserName", "UserName is required");
            
            var actual = sut.Login(loginModel, "/");

            Assert.IsInstanceOf<ViewResult>(actual);                        
        }             
        
        [Test]
        public void Should_Logoff_user()
        {
            mockedAuthenticationService.Expect(a => a.LogOff());

            var actual = sut.Logout();

            Assert.IsInstanceOf<RedirectToRouteResult>(actual);
            Assert.AreEqual("Login", ((RedirectToRouteResult)actual).RouteValues["action"]);

            mockedAuthenticationService.VerifyAllExpectations();
        }    
    }
}