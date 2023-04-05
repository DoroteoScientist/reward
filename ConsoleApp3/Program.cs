using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;


namespace ConsoleApp3
{
    class Program
    {
        //obtiene la clave y las credenciales necesarias para la api
        private const string PathToServiceAccountKeyFile = @"D:\API\key.json";
        static async Task Main(string[] args)
        {
            
            //Load the service accoun credentials and define the scope of its access
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                .CreateScoped(new[] { DriveService.ScopeConstants.Drive });
            //create the Drive Service
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential

            }
            );
            //search for text files in the directory on my account
            
            var request = service.Files.List();//enlista los archivoc del Drive
            request.Q = "parents in '16b8pe6SDQm_XzIJKCEis_xrFqFfbvtXd' and mimeType = 'text/plain'";//LE DA VALOR AL PARÁMETRO Q
            var response = await request.ExecuteAsync();
            Console.WriteLine(response);


            //List files
            foreach (var driveFile in response.Files) {
                Console.WriteLine($"{driveFile.Id}{driveFile.Name}{driveFile.MimeType}");   
            }

            //Download file 
            if (response.Files.Count(file => file.MimeType.Equals("text/plain")) > 0) { 
                var downloadFile = response.Files.FirstOrDefault(file=> file.MimeType.Equals("text/plain"));
                var getRequest = service.Files.Get(downloadFile.Id);
                await using var fileStream = new FileStream(downloadFile.Name, FileMode.Create, FileAccess.Write);
                await getRequest.DownloadAsync(fileStream);
            
            }
            //Export a Google Slide to CSV

        }

    }
}