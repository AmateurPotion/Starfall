# Starfall

## Description
Starfall는 로그라이크 기반의 멀티플레이 콘솔 RPG입니다.
다양한 시작 배경과 갈림길 선택으로 매번 다른 전개가 펼쳐집니다.
텍스트 기반 인터페이스로 직관적이면서도 깊이 있는 전략 플레이를 제공합니다.
 
# 실행방법
> dotnet run

# 핫 리로드 디버깅
> dotnet watch
실행중에도 코드 변경사항을 반영할 수 있지만, 렉 발생량이 높아
실행 중 변경을 해야만하는 상황이 아니라면 그냥 dotnet run으로
다시 디버깅하는 것을 추천합니다.

# 빌드 방법
> dotnet publish -c Release -r win-x64 --self-contained
