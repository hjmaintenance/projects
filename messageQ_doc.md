





서버는 우분투 쓸꺼임..
- (박부장님 감사합니다.)
```
sudo apt update
sudo apt install rabbitmq-server -y
sudo systemctl enable --now rabbitmq-server
sudo systemctl status rabbitmq-server
```





우리는 net8 code 에 rabbitMQ 쓸꺼임.
```
dotnet add package RabbitMQ.Client
```



## 활용 동작 방식
[Client] → API 서버 → RabbitMQ → 여러 Worker → sh 실행