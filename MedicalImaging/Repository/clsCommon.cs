using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
////using System.Threading.Tasks;

namespace MedicalImaging.Repository
{
    public class clsCommon
    {
        HttpClient client = new System.Net.Http.HttpClient();
        public string WebAPI_URL = ConfigurationManager.AppSettings["Api_Url"];




        #region Call API Post Method

        public static async Task<HttpResponseMessage> Call_API_POSTMethod(String URL, Object _referenceObject)
        {
            JsonResult JsonObj = new JsonResult();
            string url = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            using (HttpClient client = SS.Web.ServerHelper.CreateHttpClientInstance(url))
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(URL, _referenceObject);
                return response;
            }
        }

        public JsonResult Call_API_POST(String URL, Object _referenceObject)
        {
            JsonResult JsonObj = new JsonResult();
            client = new HttpClient();
            client.BaseAddress = new Uri(WebAPI_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var obj_Randomization = client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result;
            JsonObj.Data = obj_Randomization;
            return JsonObj;
        }


        //public DataTable Call_API_POSTMethod(String URL, Object _referenceObject)
        //{
        //    client = new HttpClient();
        //    client.BaseAddress = new Uri(WebAPI_URL);
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var obj_Randomization = client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result.Content.ReadAsAsync<DataTable>().Result;

        //    return obj_Randomization;
        //}

        public DataSet Call_API_POSTMethodDS(String URL, Object _referenceObject)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(WebAPI_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var obj_Randomization = client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result.Content.ReadAsAsync<DataSet>().Result;

            return obj_Randomization;
        }

        public String Call_API_POSTMethodStr(String URL, Object _referenceObject)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(WebAPI_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var obj_Randomization = client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result.Content.ReadAsAsync<String>().Result;
            return obj_Randomization;
        }

        #endregion

        #region Call API Get Method
        //public JsonResult Call_API_GETMethod(String URL)
        //{
        //    client = new HttpClient();
        //    JsonResult JsonObj = new JsonResult();
        //    client.BaseAddress = new Uri(WebAPI_URL);
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage response = client.GetAsync(URL).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = response.Content.ReadAsStringAsync().Result;
        //        JsonObj.Data = result;
        //    }
        //    JsonObj.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return JsonObj;
        //}
        public DataSet Call_API_GETMethodDS(String URL)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(WebAPI_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var obj_Randomization = client.GetAsync(URL).Result.Content.ReadAsAsync<DataSet>().Result;
            return obj_Randomization;
        }
        //public DataTable Call_API_GETMethodDT(String URL)
        //{
        //    client = new HttpClient();
        //    client.BaseAddress = new Uri(WebAPI_URL);
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var obj_Randomization = client.GetAsync(URL).Result.Content.ReadAsAsync<DataTable>().Result;
        //    return obj_Randomization;
        //}
        public async Task<DataTable> Call_API_GETMethodDT(String URL)
        {
            //client = new HttpClient();
            //client.BaseAddress = new Uri(WebAPI_URL);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var obj_Randomization = client.GetAsync(URL).Result.Content.ReadAsAsync<DataTable>().Result;
            //return obj_Randomization;

            string url = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            HttpClient client = SS.Web.ServerHelper.CreateHttpClientInstance(url);
            HttpResponseMessage response = await client.GetAsync(URL);
            return response.Content.ReadAsAsync<DataTable>().Result;
        }

        public async Task<DataTable> Call_API_GeTMethod(string URL, Object _referenceObject)
        {

            //string url = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            //HttpClient client = SS.Web.ServerHelper.CreateHttpClientInstance(url);
            //HttpResponseMessage response = await client.PostAsJsonAsync(URL, _referenceObject);
            //return response.Content.ReadAsAsync<DataTable>().Result;

            JsonResult JsonObj = new JsonResult();
            string url = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            HttpClient client = SS.Web.ServerHelper.CreateHttpClientJsonInstance(url);
            HttpResponseMessage response = await client.PostAsJsonAsync(URL, _referenceObject);
            return response.Content.ReadAsAsync<DataTable>().Result;

            //HttpResponseMessage response = await client.GetAsync(URL);
            //var data = response.Content.ReadAsStringAsync().Result;
            //JsonObj.Data = data;
            //Newtonsoft.Json.Linq.JObject results = Newtonsoft.Json.Linq.JObject.Parse(data);
           // HttpResponseMessage response = await client.PostAsJsonAsync(URL, _referenceObject);
            //return JsonObj;
        }
        #endregion

        #region DataTable To JSON
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        #endregion

        #region API For Insert Data
        public async Task<string>  Insert_APITransaction(String URL, Object _referenceObject)
        {
            //client = new HttpClient();
            //client.BaseAddress = new Uri(WebAPI_URL);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //return client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result.Content.ReadAsAsync<string>().Result;

            string url = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            HttpClient client = SS.Web.ServerHelper.CreateHttpClientInstance(url);
            HttpResponseMessage response = await client.PostAsJsonAsync(URL, _referenceObject);
            return response.Content.ReadAsAsync<string>().Result;
        }
        public async Task<DataTable> Insert_APITransactionUsingDT(String URL, Object _referenceObject)
        {
            //client = new HttpClient();
            //client.BaseAddress = new Uri(WebAPI_URL);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //return client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result.Content.ReadAsAsync<DataTable>().Result;


            string url = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            HttpClient client = SS.Web.ServerHelper.CreateHttpClientInstance(url);
            HttpResponseMessage response = await client.PostAsJsonAsync(URL, _referenceObject);
            return response.Content.ReadAsAsync<DataTable>().Result;
        }
        public DataSet Insert_APITransactionUsingDS(String URL, Object _referenceObject)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(WebAPI_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result.Content.ReadAsAsync<DataSet>().Result;
        }

        public DataTable Insert_APITransactionUsingDataTable(String URL, Object _referenceObject)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(WebAPI_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.PostAsync(URL, _referenceObject, new JsonMediaTypeFormatter()).Result.Content.ReadAsAsync<DataTable>().Result;
        }
        #endregion
    }
}