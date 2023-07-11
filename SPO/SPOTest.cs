using Microsoft.SharePoint.Client;
using PnP.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using File = Microsoft.SharePoint.Client.File;

namespace SPO
{
    public static class SPOTest
    {
        public static async Task Run()
        {
            var uniqueID = new Guid("db25253a-0ff5-471b-ab3b-cb39407b6565");
            await RenameByUniqueID(uniqueID, "hugo.png");
        }

        public static async Task RenameByFileName(string fileName)
        {
            var siteUrl = "https://75v04z.sharepoint.com/sites/FamilyTree";
            var context = await GetClientContext(siteUrl);

            Web web = context.Web;
            List library = web.Lists.GetByTitle("Avatars");
            context.Load(web);
            context.Load(library);

            // Retrieve the file
            CamlQuery query = new CamlQuery();
            query.ViewXml = $"<View Scope='RecursiveAll'><Query><Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='Text'>{fileName}</Value></Eq></Where></Query></View>";
            ListItemCollection items = library.GetItems(query);
            context.Load(items);
            context.ExecuteQuery();

            if (items.Count == 1)
            {
                // Get the file and rename it
                ListItem item = items[0];
                Microsoft.SharePoint.Client.File file = item.File;
                file.MoveTo($"Avatars/hahaha.png", MoveOperations.Overwrite);
                context.ExecuteQuery();

                Console.WriteLine("File renamed successfully.");
            }
            else
            {
                Console.WriteLine("File not found or multiple files found with the same name.");
            }
        }
        public static async Task RenameByUniqueID(Guid uniqueID, string newName)
        {
            var siteUrl = "https://75v04z.sharepoint.com/sites/FamilyTree";
            var context = await GetClientContext(siteUrl);
            Web web = context.Web;
            List library = web.Lists.GetByTitle("Avatars");

            context.Load(library);
            var items = library.GetItemByUniqueId(uniqueID);

            context.Load(items, items=> items.File);
            await context.ExecuteQueryAsync();


                File file = items.File;
                file.MoveTo($"Avatars/{newName}", MoveOperations.Overwrite);
                context.ExecuteQuery();
        }

        public static async Task GetUniqueID(string fileName)
        {
            var siteUrl = "https://75v04z.sharepoint.com/sites/FamilyTree";
            var context = await GetClientContext(siteUrl);
            Web web = context.Web;
            List library = web.Lists.GetByTitle("Avatars");

            CamlQuery query = new CamlQuery();
            query.ViewXml = $"<View Scope='RecursiveAll'><Query><Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='Text'>{fileName}</Value></Eq></Where></Query></View>";

            ListItemCollection items = library.GetItems(query);
            context.Load(items);
            context.ExecuteQuery();

            if (items.Count == 1)
            {
                ListItem item = items[0];
                File file = item.File;
                context.Load(file, f => f.UniqueId);
                context.ExecuteQuery();
                Console.WriteLine(file.UniqueId.ToString());
            }
            else if (items.Count == 0)
            {
                throw new Exception("File not found in the document library.");
            }
            else
            {
                throw new Exception("Multiple files with the same name found in the document library.");
            }

        }

        public static async Task<ClientContext> GetClientContext(string siteUrl)
        {
            AuthenticationManager authen = new AuthenticationManager(setting.clientId, setting.userName, ConvertToSecureString(setting.password));
            return await authen.GetContextAsync(siteUrl);
        }

        private static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}
