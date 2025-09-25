#!/bin/bash
# 사용법: ./wrkRecept.sh "제목내용" "본문내용"

# 파라미터
TITLE="$1"
BODY="$2"

# 파일명 (날짜시간)
FILENAME="$(date '+%Y%m%d_%H%M%S').txt"

echo $TITLE
echo $BODY

echo $FILENAME

# 메시지 작성 및 저장
echo "wrkRecept.sh" > "/home/quri/projects/msgQ/$FILENAME"


# msgQ 디렉토리로 이동
cd /home/quri/projects/msgQ || exit 1

# 오래된 파일 정리 (48시간 지난 txt 파일 삭제 2880 ) 실 서비스 전 까지 10분으로 처리 하자 쌓인다..
find . -maxdepth 1 -type f -name "*.txt" -mmin +10 -exec rm -f {} \;

# git add & commit & push
# git add "$FILENAME"
git add .
git commit -m "reception_msg:${TITLE}" -m "${BODY}"
git pull
git push # -f origin main