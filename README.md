# azure-functions-call-computer-vsion

This is a demo about using Azure Functions Call computer vision;

Call API:http://localhost:7071/api/HttpTriggerCSharp1?filepath=https://ppblobsean.blob.core.windows.net/ppfiles/2021-04-27-15-43-55.jpg

FilePath should be a public img path.

The functios in folder “http-trigger-computer-version-container” support docker running locally：

Run Commond：using -e to pass parameters

docker build -t myfuncforpowerapp .  

docker run -p 8080:80 -e ComputerVisionSubscriptionKey="xxxxx" -e ComputerVisionEndpoint="https://xxxxx.cognitiveservices.azure.com/"  myfuncforpowerapp 
