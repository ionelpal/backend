using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using nccloudService.DataObjects;
using nccloudService.Models;
using System.Security.Principal;
using Microsoft.Azure.Mobile.Server.Authentication;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace nccloudService.Controllers
{   [Authorize]
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            nccloudContext context = new nccloudContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request);
        }
        //=================
        private string GoogleSID()      
        {
            var principal = this.User as ClaimsPrincipal;
            var sid = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            return sid;
        }



        private async Task<string> GetEmailAddress()
        {
            var credentials = await User.GetAppServiceIdentityAsync<GoogleCredentials>(Request);
            return credentials.UserClaims
                .Where(claim => claim.Type.EndsWith("/emailaddress"))
                .First<Claim>()
                .Value;
           // Debug.WriteLine(credentials.UserClaims);
        }
        //==================
        // GET tables/TodoItem
        public async Task<IQueryable<TodoItem>> GetAllTodoItems()
        {
            Debug.WriteLine("GET tables/TodoItem");
            var emailAddr = await GetEmailAddress();
            return Query().Where(item => item.UserId == emailAddr);
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<SingleResult<TodoItem>> GetTodoItem(string id)
        {
            Debug.WriteLine($"GET tables/TodoItem/{id}");
            var emailAddr = await GetEmailAddress();
            var result = Lookup(id).Queryable.Where(item => item.UserId == emailAddr);
            return new SingleResult<TodoItem>(result);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            Debug.WriteLine($"PATCH tables/TodoItem/{id}");
            var item = Lookup(id).Queryable.FirstOrDefault<TodoItem>();
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var emailAddr = await GetEmailAddress();
            if (item.UserId != emailAddr)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            return await UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            Debug.WriteLine($"POST tables/TodoItem");
            var emailAddr = await GetEmailAddress();
            item.UserId = emailAddr;
            TodoItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

    

    // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
    public async Task DeleteTodoItem(string id)
        {
            Debug.WriteLine($"DELETE tables/TodoItem/{id}");
            var item = Lookup(id).Queryable.FirstOrDefault<TodoItem>();
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var emailAddr = await GetEmailAddress();
            if (item.UserId != emailAddr)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
           await DeleteAsync(id);
           // return DeleteAsync(id);
        }
    }
}