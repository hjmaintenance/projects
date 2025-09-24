https://github.com/jsiniboss/projects
가 team project 로 변경 되었습니다.
https://github.com/hjmaintenance/projects

기존소스의 git 정보를 아래 명령으로 변경 처리 하실수 있습니다.

git remote set-url origin https://github.com/hjmaintenance/projects.git

git branch --set-upstream-to=origin/main main

git config pull.rebase false

git fetch origin

git pull origin main
