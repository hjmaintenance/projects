using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Extensions.Logging;

public class ScriptRunner
{
    public static async Task Main(string[] args)
    {
        // 로깅 설정
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<ScriptRunner>();

        // RabbitMQ 연결 설정
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // 큐 선언
        await channel.QueueDeclareAsync(queue: "run_script",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        // 한 번에 하나의 메시지만 처리하도록 설정
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        logger.LogInformation(" [*] Waiting for messages. To exit press CTRL+C");

        // 소비자(Consumer) 설정
        var consumer = new AsyncEventingBasicConsumer(channel);

        // 메시지 수신 시 호출될 이벤트 핸들러
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            logger.LogInformation(" [x] Received {Message}", json);

            try
            {
                logger.LogInformation(" start");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var payload = JsonSerializer.Deserialize<ScriptMessage>(json, options);

                logger.LogInformation(" payload is null :{Message}", (payload == null) ? "yes" : "no");

                if (payload != null && !string.IsNullOrEmpty(payload.Script))
                {


                    logger.LogInformation(" if in 1", payload.Script);
                    //logger.LogInformation(" if in 2", payload.Args);


                    string body_str = payload.Args[1];
                    // 이미지 추출 및 CID 치환
                    var (newBody, attachments) = ProcessBase64Images2(body_str);


                    string scriptPath = payload.Script;
                    string title = payload.Args[0];

                    logger.LogInformation(" scriptPath {Message}", scriptPath);
                    logger.LogInformation(" title: {Message}", title);
                    logger.LogInformation(" newBody: {Message}", newBody);

                    // ArgumentList를 사용하면 공백이나 특수문자가 포함된 인자를 안전하게 전달할 수 있습니다.
                    // 운영체제가 각 인자를 올바르게 처리하도록 보장합니다.
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = scriptPath, // /bin/bash 대신 스크립트 경로를 직접 지정
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    startInfo.ArgumentList.Add(title);
                    startInfo.ArgumentList.Add(newBody);

                    var process = new Process
                    {
                        StartInfo = startInfo
                    };

                    process.Start();

                    // 비동기적으로 표준 출력과 표준 에러를 읽습니다.
                    Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                    Task<string> errorTask = process.StandardError.ReadToEndAsync();

                    // 스크립트가 완료될 때까지 기다립니다.
                    await process.WaitForExitAsync();

                    string output = await outputTask;
                    string error = await errorTask;

                    if (!string.IsNullOrEmpty(output))
                    {
                        logger.LogInformation("Script output: {Output}", output);
                    }

                }
                else
                {
                    logger.LogWarning("Invalid payload received: {Payload}", json);
                }

                // 메시지 처리 완료를 RabbitMQ에 알립니다.
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message: {Message}", json);
                // 메시지 처리에 실패했음을 알리고, 큐에 다시 넣지 않습니다(false).
                await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        // 소비 시작
        await channel.BasicConsumeAsync(queue: "run_script",
                             autoAck: false, // 수동 확인 모드
                             consumer: consumer);

        // 프로그램이 종료되지 않도록 무한 대기
        await Task.Delay(Timeout.Infinite);
    }
    /*
        static (string, List<LinkedResource>) ProcessBase64Images(string body)
        {
            var linkedResources = new List<LinkedResource>();
            string pattern = @"<img[^>]+src=""data:(image\/[a-zA-Z]+);base64,([^""]+)""";

            int index = 0;
            string newBody = Regex.Replace(body, pattern, match =>
            {
                string mimeType = match.Groups[1].Value; // e.g. image/png
                string base64Data = match.Groups[2].Value;
                byte[] bytes = Convert.FromBase64String(base64Data);

                // 확장자 결정
                string extension = mimeType switch
                {
                    "image/png" => ".png",
                    "image/jpeg" => ".jpg",
                    "image/gif" => ".gif",
                    _ => ".bin"
                };

                string fileName = $"image_{index}{extension}";
                File.WriteAllBytes(fileName, bytes);

                // CID 생성
                string cid = Guid.NewGuid().ToString();
                var resource = new LinkedResource(fileName, mimeType)
                {
                    ContentId = cid,
                    TransferEncoding = TransferEncoding.Base64
                };
                linkedResources.Add(resource);

                index++;
                return $"<img src=\"cid:{cid}\" alt=\"inline-img\" />";
            });

            return (newBody, linkedResources);
        }
    */
    static (string, List<(string FileName, string Cid)>) ProcessBase64Images2(string body)
    {
        var attachments = new List<(string, string)>();
        string pattern = @"<img[^>]+src=""data:(image\/[a-zA-Z]+);base64,([^""]+)""";

        int index = 0;
        string attachmentsStr = "_quristart_";
        string newBody = Regex.Replace(body, pattern, match =>
        {
            string mimeType = match.Groups[1].Value; // image/png
            string base64Data = match.Groups[2].Value;
            byte[] bytes = Convert.FromBase64String(base64Data);

            // 확장자 추출
            string extension = mimeType switch
            {
                "image/png" => ".png",
                "image/jpeg" => ".jpg",
                "image/gif" => ".gif",
                _ => ".bin"
            };

            string dtname = DateTime.Now.Ticks.ToString() + "_";

            string fileName = "/home/quri/projects/msgQ/" + dtname + $"image_{index}{extension}";
            File.WriteAllBytes(fileName, bytes);

            string cid = dtname + $"img{index}@cid";
            attachments.Add((fileName, cid));

            index++;
            attachmentsStr += fileName.Replace("/home/quri/projects/", ".") + ";" + cid + ",";
            return $"<img src=\"cid:{cid}\" alt=\"inline-img\" />";
        });


        attachmentsStr += "_quriend_";

        newBody = newBody + attachmentsStr;

        return (newBody, attachments);
    }


}

// DTO 정의
public class ScriptMessage
{
    public string Script { get; set; } = string.Empty;
    public string[] Args { get; set; } = Array.Empty<string>();
}
