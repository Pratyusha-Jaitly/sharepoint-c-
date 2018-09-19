using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace firstConsoleApplication1
{
    class Program
    {
        static string url = 
            "https://abcztcs.sharepoint.com/sites/o365kb";
        static string username = "pratyusha@abcztcs.onmicrosoft.com";
        static string pwd = "Tata@1234";

        static ClientContext GetClientContext()
        {
            SecureString password = new SecureString();

            foreach (var item in pwd.ToCharArray())
            {
                password.AppendChar(item);
            }
            SharePointOnlineCredentials creds = 
                new SharePointOnlineCredentials(username, password);

            ClientContext context =
                new ClientContext(url);
            context.Credentials = creds;
            return context;
            // Site siteColl = context.Site;//Site Collection
        }
        
        static void ShowLists()
        {
            ClientContext context = GetClientContext();
            Web site = context.Site.RootWeb;


            //Listcollections
            ListCollection lists = context.Web.Lists;
            context.Load(lists);
            context.ExecuteQuery();
            foreach (var item in lists)
            {
                Console.WriteLine(item.Title);
            }
        }

        static void CreateList()
        {
            ClientContext context = GetClientContext();
            //Meta Info
            ListCreationInformation listInfo =
                new ListCreationInformation();
            listInfo.Title = "pratCustomList";
            listInfo.TemplateType = (int)ListTemplateType.GenericList;
            listInfo.QuickLaunchOption = 
                QuickLaunchOptions.DefaultValue;
           
            ListCollection lists = context.Web.Lists;
            List customList = lists.Add(listInfo);

            //columns
            // one way of doing this -customList.Fields.add
            customList.Fields.AddFieldAsXml(
                "<Field DisplayName='ProductID' Type='Number'/>",
                true,
                AddFieldOptions.DefaultValue);
            customList.Fields.AddFieldAsXml(
                "<Field DisplayName='ProductName' Type='Text'/>",
               true,
               AddFieldOptions.DefaultValue);
            customList.Fields.AddFieldAsXml(
                "<Field DisplayName='ProductQuantity' Type='Number'/>",
               true,
               AddFieldOptions.DefaultValue);
            customList.Fields.AddFieldAsXml(
                "<Field DisplayName='ProductPrice' Type='Currency'/>",
               true,
               AddFieldOptions.DefaultValue);
            context.ExecuteQuery();
            Console.WriteLine("List Created");
             
        }
        static void AddListItem()
        {
            ClientContext context = GetClientContext();
            List customList = context.Web.Lists.GetByTitle("pratCustomList");
            ListItemCreationInformation itemInfo =
                new ListItemCreationInformation();
           
            ListItem item = customList.AddItem(itemInfo);
            item["ProductID"] = 1;
            item["ProductName"] = "Maggi";
            item["ProductQuantity"] = 5;
            item["ProductPrice"] = 30.0;
            item.Update();
    
            customList.Update();
            context.ExecuteQuery();
            Console.WriteLine("Value Added");
                
        }
        static void ShowListItem()
        {
            ClientContext context = GetClientContext();
            ListCollection lists = context.Web.Lists;
            List deptList = lists.GetByTitle("pratTrainTravel");
            FieldCollection fields = deptList.Fields;

            ListItemCollection items = deptList.GetItems
                (CamlQuery.CreateAllItemsQuery());



            context.Load(deptList);
            context.Load(fields);
            context.Load(items);
            context.ExecuteQuery();
            //foreach (var item in fields)
            //{

            //    Console.WriteLine(item.Title);
            //}
            foreach (var row in items)

            {
                Console.WriteLine(row["TravelID"]+"");

            }
        }
        static void Main(string[] args)
        {

            // ShowLists();
            //ShowListItem();
            // CreateList();
            AddListItem();
            
        }
    }
}
