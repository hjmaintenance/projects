#!/bin/bash


echo " "

# backend 중단
echo "=== 기존 백엔드 프로세스 중단 ==="
pkill -f "dotnet.*JinRestApi.dll"

sleep 3   # 안전하게 2초 대기




# 환경 변수에서 경로를 읽어오거나 기본값을 사용
ReleseQueue_PATH="${ReleseQueue__Path:-/home/lee/projects}"
Front_PATH="${Front__Path:-/home/lee/projects/JinReception}"
Backend_PATH="${Backend__Path:-/home/lee/projects/JinRestApi}"



# 공통 버전 생성 (날짜 기반 예시)
RELEASE_VERSION="0.7.$(date +%y%m%d%H%M)"
# echo $RELEASE_VERSION > VERSION

echo "=== 릴리즈 버전: $RELEASE_VERSION ==="

### Frontend 반영 ###

# version.json 경로
VERSION_FILE="$Front_PATH/dist/version.json"

# 디렉토리 존재 확인, 없으면 생성
mkdir -p "$(dirname "$VERSION_FILE")"

# version.json 생성/갱신
echo "{ \"version\": \"$RELEASE_VERSION\" }" > "$VERSION_FILE"

echo "Frontend version.json 생성/갱신 완료: $VERSION_FILE"


# projects 디렉토리로 이동
cd "$ReleseQueue_PATH" || exit 1


echo "=== Source Check ==="

# source get 
git pull

# projects/JinReception 디렉토리로 이동
cd "$Front_PATH" || exit 1

# 프론트 빌드

echo "=== 프론트 빌드 ==="
npm run build

sleep 1   # 안전하게 1초 대기


### Backend 반영 ###

# projects/JinRestApi 디렉토리로 이동
cd "$Backend_PATH" || exit 1

# 백엔드 빌드
# dotnet publish -c Release
# dotnet run

# dotnet publish → 실제 실행파일 생성

echo "=== 백엔드 빌드 ==="
dotnet publish -c Release -o ./publish

sleep 1   # 안전하게 1초 대기

# 백엔드 재실행 (백그라운드 실행 + 로그 저장)
echo "=== 백엔드 시작 ==="
LOGFILE="../logs/backend_$(date +%Y%m%d_%H%M).log"



nohup dotnet ./publish/JinRestApi.dll > "$LOGFILE" 2>&1 &



