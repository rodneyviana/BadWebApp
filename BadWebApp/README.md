# BadWebApp
This is a debugging project. No point in cloning unless you are training debugging.

Create an Azure SQL Server from the Demo Template

Create a Azure Function using Runtime ~3 and create an Application Setting:
(Function App Resource | Configuration | Application Settings)

Name it 'AzureSqlConnection' and make the value the connection string you got from your Azure SQL configuration
Make sure you add the password to the string.
Example:
From:
```
Server=tcp:contososql.database.windows.net,1433;Initial Catalog=ContosoAppSQL;Persist Security Info=False;User ID=mysystemadmin;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```
Replace {your_password} by your actual password (please don't choose Password123 as your password')
```
Server=tcp:contososql.database.windows.net,1433;Initial Catalog=ContosoAppSQL;Persist Security Info=False;User ID=mysystemadmin;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

In your function, add this code:
```
#r "Newtonsoft.Json"
#r "System.Data.SqlClient"

using System.Net;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Data.SqlClient;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    const string error = "Category filter cannot contain ' or ;";
    log.LogInformation("C# HTTP trigger function processed a request.");

    string cat = req.Query["cat"];
    if(null != cat && (cat.Contains("'") || cat.Contains(";")))
    {
        log.LogError(error);
        return new BadRequestObjectResult($"{{ \"error\": \"{error}\" }}");
    }
    string conn = Environment.GetEnvironmentVariable("AzureSqlConnection");

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    cat = cat ?? data?.cat;
    using (SqlConnection connection = new SqlConnection(conn))
    {
        string query = null;
        if(String.IsNullOrEmpty(cat))
        {
            query = "SELECT CAST((SELECT * FROM SalesLT.vGetAllCategories FOR JSON PATH) AS VARCHAR(MAX))";
        } else
        {
            query = "SELECT CAST((SELECT * FROM SalesLT.vGetAllCategories WHERE ProductCategoryName LIKE @Cat FOR JSON PATH) AS VARCHAR(MAX))";
        }
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            if(query.Contains("@Cat"))
            {
                cmd.Parameters.AddWithValue("@Cat", "%"+cat+"%");
            }
            connection.Open();
            string result = null;
            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    result = reader[0] as string;
                    break;
                }
            }
            log.LogInformation("End: "+result.Substring(result.Length - 50));
            return new OkObjectResult(result);
        }
    } 
}

```

To get instrumentation for your pages, use the snippet below (original) in github:

https://github.com/Microsoft/ApplicationInsights-JS#snippet-setup-ignore-if-using-npm-setup

```
<script type="text/javascript">
!function(T,l,y){var S=T.location,k="script",D="instrumentationKey",C="ingestionendpoint",I="disableExceptionTracking",E="ai.device.",b="toLowerCase",w="crossOrigin",N="POST",e="appInsightsSDK",t=y.name||"appInsights";(y.name||T[e])&&(T[e]=t);var n=T[t]||function(d){var g=!1,f=!1,m={initialize:!0,queue:[],sv:"5",version:2,config:d};function v(e,t){var n={},a="Browser";return n[E+"id"]=a[b](),n[E+"type"]=a,n["ai.operation.name"]=S&&S.pathname||"_unknown_",n["ai.internal.sdkVersion"]="javascript:snippet_"+(m.sv||m.version),{time:function(){var e=new Date;function t(e){var t=""+e;return 1===t.length&&(t="0"+t),t}return e.getUTCFullYear()+"-"+t(1+e.getUTCMonth())+"-"+t(e.getUTCDate())+"T"+t(e.getUTCHours())+":"+t(e.getUTCMinutes())+":"+t(e.getUTCSeconds())+"."+((e.getUTCMilliseconds()/1e3).toFixed(3)+"").slice(2,5)+"Z"}(),iKey:e,name:"Microsoft.ApplicationInsights."+e.replace(/-/g,"")+"."+t,sampleRate:100,tags:n,data:{baseData:{ver:2}}}}var h=d.url||y.src;if(h){function a(e){var t,n,a,i,r,o,s,c,u,p,l;g=!0,m.queue=[],f||(f=!0,t=h,s=function(){var e={},t=d.connectionString;if(t)for(var n=t.split(";"),a=0;a<n.length;a++){var i=n[a].split("=");2===i.length&&(e[i[0][b]()]=i[1])}if(!e[C]){var r=e.endpointsuffix,o=r?e.location:null;e[C]="https://"+(o?o+".":"")+"dc."+(r||"services.visualstudio.com")}return e}(),c=s[D]||d[D]||"",u=s[C],p=u?u+"/v2/track":d.endpointUrl,(l=[]).push((n="SDK LOAD Failure: Failed to load Application Insights SDK script (See stack for details)",a=t,i=p,(o=(r=v(c,"Exception")).data).baseType="ExceptionData",o.baseData.exceptions=[{typeName:"SDKLoadFailed",message:n.replace(/\./g,"-"),hasFullStack:!1,stack:n+"\nSnippet failed to load ["+a+"] -- Telemetry is disabled\nHelp Link: https://go.microsoft.com/fwlink/?linkid=2128109\nHost: "+(S&&S.pathname||"_unknown_")+"\nEndpoint: "+i,parsedStack:[]}],r)),l.push(function(e,t,n,a){var i=v(c,"Message"),r=i.data;r.baseType="MessageData";var o=r.baseData;return o.message='AI (Internal): 99 message:"'+("SDK LOAD Failure: Failed to load Application Insights SDK script (See stack for details) ("+n+")").replace(/\"/g,"")+'"',o.properties={endpoint:a},i}(0,0,t,p)),function(e,t){if(JSON){var n=T.fetch;if(n&&!y.useXhr)n(t,{method:N,body:JSON.stringify(e),mode:"cors"});else if(XMLHttpRequest){var a=new XMLHttpRequest;a.open(N,t),a.setRequestHeader("Content-type","application/json"),a.send(JSON.stringify(e))}}}(l,p))}function i(e,t){f||setTimeout(function(){!t&&m.core||a()},500)}var e=function(){var n=l.createElement(k);n.src=h;var e=y[w];return!e&&""!==e||"undefined"==n[w]||(n[w]=e),n.onload=i,n.onerror=a,n.onreadystatechange=function(e,t){"loaded"!==n.readyState&&"complete"!==n.readyState||i(0,t)},n}();y.ld<0?l.getElementsByTagName("head")[0].appendChild(e):setTimeout(function(){l.getElementsByTagName(k)[0].parentNode.appendChild(e)},y.ld||0)}try{m.cookie=l.cookie}catch(p){}function t(e){for(;e.length;)!function(t){m[t]=function(){var e=arguments;g||m.queue.push(function(){m[t].apply(m,e)})}}(e.pop())}var n="track",r="TrackPage",o="TrackEvent";t([n+"Event",n+"PageView",n+"Exception",n+"Trace",n+"DependencyData",n+"Metric",n+"PageViewPerformance","start"+r,"stop"+r,"start"+o,"stop"+o,"addTelemetryInitializer","setAuthenticatedUserContext","clearAuthenticatedUserContext","flush"]),m.SeverityLevel={Verbose:0,Information:1,Warning:2,Error:3,Critical:4};var s=(d.extensionConfig||{}).ApplicationInsightsAnalytics||{};if(!0!==d[I]&&!0!==s[I]){var c="onerror";t(["_"+c]);var u=T[c];T[c]=function(e,t,n,a,i){var r=u&&u(e,t,n,a,i);return!0!==r&&m["_"+c]({message:e,url:t,lineNumber:n,columnNumber:a,error:i}),r},d.autoExceptionInstrumented=!0}return m}(y.cfg);function a(){y.onInit&&y.onInit(n)}(T[t]=n).queue&&0===n.queue.length?(n.queue.push(a),n.trackPageView({})):a()}(window,document,{
src: "https://js.monitor.azure.com/scripts/b/ai.2.min.js", // The SDK URL Source
// name: "appInsights", // Global SDK Instance name defaults to "appInsights" when not supplied
// ld: 0, // Defines the load delay (in ms) before attempting to load the sdk. -1 = block page load and add to head. (default) = 0ms load after timeout,
// useXhr: 1, // Use XHR instead of fetch to report failures (if available),
crossOrigin: "anonymous", // When supplied this will add the provided value as the cross origin attribute on the script tag
// onInit: null, // Once the application insights instance has loaded and initialized this callback function will be called with 1 argument -- the sdk instance (DO NOT ADD anything to the sdk.queue -- As they won't get called)
cfg: { // Application Insights Configuration
    instrumentationKey: "INSTRUMENTATION_KEY"
}});
</script>
```

It is better to insert the snippet in the master page.