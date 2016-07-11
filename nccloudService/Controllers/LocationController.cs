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
{
    [Authorize]
    public class LocationController : TableController<Location>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            nccloudContext context = new nccloudContext();
            DomainManager = new EntityDomainManager<Location>(context, Request);
        }

        //get user email address
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




        // GET tables/Location
        public async Task<IQueryable<Location>> GetAllLocation()
        {
            var emailAddr = await GetEmailAddress();
           
            
                   return Query().Where(i=>i.Custommers.All(item=>item.CustomerEmail==emailAddr));
        }

        // GET tables/Location/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Location> GetLocation(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Location/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Location> PatchLocation(string id, Delta<Location> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Location
        public async Task<IHttpActionResult> PostLocation(Location item)
        {
            Location current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Location/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteLocation(string id)
        {
             return DeleteAsync(id);
        }
    }
}
