# Jin Reception

## Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Usage](#usage)
- [Contributing](../CONTRIBUTING.md)

## About <a name = "about"></a>

장애, 개선 요청 접수 시스템 의 Front.
1. 가볍고 빠르게
2. 심플한 디자인
3. 처리의 확인은 필수

## Getting Started <a name = "getting_started"></a>

ubuntu 24.04 버전으로 설명 합니다. 다른운영체제 사용하시는 분은 아래에 추가로 부탁 합니다.

### Prerequisites

필수툴 설치. 자신의 환경에서 최신화 추천.

```
sudo apt update
sudo apt upgrade -y
sudo apt install -y git curl build-essential python3 ca-certificates
```

### Installing

nvm, node 설치

```
# nvm 설치
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.4/install.sh | bash

# 설치 후 셸 환경 재로딩 
source ~/.bashrc   # source ~/.profile , source ~/.zshrc

# LTS 설치
nvm install --lts
nvm use --lts

# 확인
node -v
npm -v

```

소스 가져오기

```
git clone https://github.com/jsiniboss/projects.git
cd projects

# 의존성 설치

npm install
```

개발서버 실행
```
npm run dev
```




## Usage <a name = "usage"></a>

vue3, tailwind, jquery


## vscode 확장 추천

- Tailwind Css intellisense 
- Volar
- EsLint
- Prettier-Code formatter
- Vue