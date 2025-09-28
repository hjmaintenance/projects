## 설명서





마이그레이션 도구 dotnet-ef 설치
```
dotnet tool install --global dotnet-ef

```

마이그레이션 실행
```
dotnet ef migrations add InitTable
dotnet ef database update

```


- 이게 잘 안되시는 분이 계실텐데.. 아무도 문의가 없긴 하군요.. ^^

- 이것은 마이그레이션시 초기 테이블 생성 코드가 만들어 지기 때문입니다.
- 이래서 프로젝트때 한사람이 도 맡아서 지시 하는데..
- 우리는 누구나 처리 가능한 상태(능력)를 가지는게 목표 입니다.

- 마이그레이션의 설정을 아래와 같이 준비 합니다.





## Step by step


1. Migrations 폴더 삭제


2. 마이그레이션 초기 명령으로 최초 파일들을 생성
```
dotnet ef migrations add InitialCreate
```

3. Migrations 폴더 안에   날짜시간..._InitialCreate.cs 파일의 UP method 의 내용물 모두 삭제.. up 껍데기만 남겨 둔다.
아래와 같은 모습으로.
```

/// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }  
```

3.1 up 을 지우는게 부담된다면 아래 쿼리를 실행후 up 그대로 두고 아래 절차를 따른다.
```
 DELETE FROM jsini."__EFMigrationsHistory";
```

4. 껍데기 마이그레이션을 한다.
```
dotnet ef database update
```

5. 실제 마이그레이션을 한다. 
```
dotnet ef migrations add 영문과숫자를이용한제목정도
```

6. db 에 반영. 
```
dotnet ef database update
```

7. 이후 부터는 JinRestApi 폴더에서 아래 명령을 이용하여 사용한다.
```
dotnet ef migrations add 영문과숫자를이용한제목정도
dotnet ef database update

```




