# Unity-AutoDefence
 
<h1>이동규 2015180030 게임공학과</h1>

<h1>게임 소개</h1>
매 스테이지마다 등장하는 적을 유닛을 구입, 배치하여 방어하는 게임입니다.
타워디펜스와 비슷하지만 적이 일정한 경로를 따라 움직이지 않고
간단한 인공지능으로 서로 전투하는것을 구현했습니다.
모델링을 할 수 없어 에셋을 사용하기 위해 구하기 쉬운 로우폴리곤 에셋들을 사용하였습니다

시작시 3골드가 주어지며 매 스테이지가 끝날때마다 2골드씩 추가로 얻게됩니다.
유닛 구입, 새로고침에는 1골드씩 소모가 되며
스테이지가 끝날때 자동으로 새로고침이 이루어집니다.

적은 매 스테이지마다 스테이지 숫자만큼의 웨이브로 등장하며
6마리씩 웨이브마다 등장하게됩니다 <br>
ex) <br>
1스테이지 - 1웨이브 - 총 6마리 <br>
2스테이지 - 2웨이브 - 총 12마리 <br>
...<br>
4스테이지 - 4웨이브 - 총 24마리 <br>
그러나 5웨이브마다 해당 웨이브는 적 6마리가 등장하지 않고 더 강한 적이 1마리 등장합니다 <br>

<h2>조작키</h2>
카메라 움직임을 구현하여 마우스 우클릭후 마우스 움직임에 따라 카메라의 방향이 바뀌며<br>
wasd, 화살표 방향키 - 카메라 이동<br>
q, e - 카메라 높낮이<br>
마우스 휠 - 카메라 확대 축소<br>
백스페이스 - 카메라 원위치<br>

오브젝트를 클릭하면 화면의 좌측 상단에 해당 오브젝트의 정보가 표시되며
스테이지 시작전에는 오브젝트를 드래그해서 위치를 바꿔줄 수 있으며
맵 구석에 Trash Box에 이동시키면 오브젝트가 제거되며 빈자리가 생기게 됩니다.

기본적으로 카메라 조작 외에는 별다른 조작을 하지 않으며
스테이지 시작 전 유닛을 구입, 배치하고 스테이지 시작 버튼을 누르게 되면
스테이지 동안에는 카메라를 움직이면서 전투하는것을 지켜보는 게임입니다.

방어 오브젝트는 이동속도, 행동반경, 공격사거리, 공격속도 등등 능력치가 모두 다르며
방어 오브젝트들은 각각의 스킬이 하나씩 존재합니다.

<h2>방어 오브젝트 종류</h2>
Golem -<br>
체력이 가장 높으나 이동속도나 공격력, 공격속도가 가장 낮아 적의 공격을 받아내는 용도로만
활용이 가능합니다<br>
스킬 :  매 공격시 자신의 체력을 조금씩 회복합니다<br>

Knight -<br>
체력은 두번째로 높고 나머지 능력치는 무난한 편의 근접 유닛으로 적의 공격을 받아내며
공격이 가능합니다<br>
스킬 : 자신의 체력을 크게 회복시킵니다.<br>

DogKnight -<br>
체력은 조금 낮지만 공격속도가 빠르고 공격력이 강하고 사거리가 조금 길어 2선의 위치에서
공격을 꾸준하게 해줄 수 있습니다.<br>
스킬 : 자신의 공격속도를 일정시간동안 큰 폭으로 증가시킵니다.<br>

Solider - <br>
체력은 가장 낮지만 가장 넓은 공격 사거리와 강한 공격력으로 맨 뒤에서
꾸준하게 적을 공격합니다<br>
스킬 : 자신의 공격력을 일정시간동안 큰 폭으로 증가시킵니다.<br>

MachineGun -<br>
무난한 능력치와 가장 낮은 데미지, 가장 빠른 공격속도를 가지고있습니다.<br>
스킬 : 자신의 공격속도를 일정시간동안 증가시킵니다.<br>

Bazooka - <br>
가장 높은 공격력과 가장 느린 공격속도를 가지고 어느정도 짧은 사거리를 가지고있습니다<br>
스킬 : 전방에 피해를 주는 폭발을 발사합니다.<br>

BomberMan -<br>
가장 무난한 능력치를 가지고있는 원거리 유닛입니다<br>
스킬 : 자신 주변에 폭탄을 떨어트려 각각 피해를 줍니다.<br>

기획상으론 시너지효과를 넣으려고 했지만 무엇보다도 게임에 사용할 애니메이션이 있는
모델링을 무료로 구하기가 너무 어려워서 7가지밖에 넣지 못했고 7가지로 어떻게
구현해야할지 난감해서 넣지 않았습니다.. 또한 애니메이션이
대부분 죽는 애니메이션이 없어서 피가 없어지면 바로 제거를 시켰습니다.

<h1>게임에 적용된 기술</h1>
유닛끼리 전투할때 특정 태그의 오브젝트들을 반환받아 이를
자신과의 거리순으로 정렬하여 가장 가까운 적을 공격하게 하기 위해
람다함수의 역활을 하는 Delegate 를 사용하여 정렬기준을 구현하였습니다.

원거리 공격의 경우 총알 발사체를 발사하여 해당 발사체가 충돌했을때 데미지를
가하게 구현하였습니다.
또한 공격시 파티클, 타격시 파티클 등등 기본 파티클 오브젝트를 이용하여 구현하였습니다.

피킹을 통해 스테이지 시작 전 오브젝트들의 위치를 바꿔줄 수 있으며
해당 오브젝트의 정보창을 띄우거나 유닛을 구입하는 등 다양한 요소에 사용하였습니다.

애니메이션 컨트롤러를 사용하여 오브젝트들의 Idle, Run, Attack 상태를 트리거로
구현하여 연결해주었습니다. 또한 반복되는 애니메이션때문에 모션이 어색해 보일 수 있어
Run과 같은 애니메이션은 일정 시간동안 애니메이션의 상태를 바꾸지 못하도록
스크립트에서 처리해주었습니다

오브젝트끼리 전투할때 상대방으로 다가감과 동시에 해당 방향을 바라보는 것을
자연스럽게 처리 하기 위해서
방향벡터를 Vector3의 Slerp 함수를 이용하여 보간하여 회전시켰습니다.

오브젝트들은 모두 Rigidbody를 적용하여 지형지물 위에서 자연스럽게 움직이도록 하였습니다.
그러나 피킹을 통해 오브젝트를 이동시키다 충돌하게 된다면 오브젝트가 날라가버리는 상황이
발생하여 특정 조건에서는 isKinemetic 을 켜서 물리충돌을 일으키지 않도록 예외처리하였습니다.

오브젝트들의 움직임에 모두 deltaTime을 사용하여 프레임변화에도 일정한 움직임을
보여주도록 구현하였습니다. 
적군의 스폰방식은 코루틴을 사용하여 스테이지 숫자만큼 반복하여 주기적으로 스폰합니다.
또한 에너미 스포너나 게임매니져같은 오브젝트는 싱글톤으로 처리하였습니다.

<h2>강의에서 진행한 내용 중 사용된 부분</h2>
이번 강의를 통해 유니티를 처음 접했고 그 전에는 엔진에 대한 기반 지식이 없었습니다.
개발 초기에 구현한 이미지 게임 오브젝트 클릭같은 경우는 구글링을 통해 구현하였으나
그 후에는 Raycast 피킹 방식을 사용하는 등 모든 기능들은 이번 학기동안 들은 강의를 통해서 
전부 구현하였습니다.
컨텐츠쪽의 기능들은 엔진 사용은 처음이지만 DirectX나 OpenGL은 다뤄보았기 때문에
C#을 조금 익히고 사용했었습니다.

또한 외부에셋은 어떻게 가져와서 제대로 사용하는지 잘 모르겠어서
텍스쳐나 모델링, 애니메이션만 활용하였으며 그 외 스크립트부분은 타이틀씬의 
메뉴UI나 우측 상단에 프레임및 정보를 표시해주는 UI 에셋 외에는
직접 구현하였습니다.


<h1>사용한 에셋 출처</h1>

적군 오브젝트 Titan<br>
https://assetstore.unity.com/packages/3d/characters/creatures/creature-titan-79302

Golem 모델 및 애니메이션<br>
https://assetstore.unity.com/packages/3d/characters/humanoids/mini-legion-rock-golem-pbr-hp-polyart-94707

DogKnight 모델 및 애니메이션<br>
https://assetstore.unity.com/packages/3d/characters/animals/dog-knight-pbr-polyart-135227

Knight 모델 및 애니메이션<br>
https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy-chess-rpg-character-arthur-160647

폭발 모델링<br>
https://assetstore.unity.com/packages/tools/particles-effects/true-explosions-21893

Frame UI ( Graphy )<br>
https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-fps-counter-stats-monitor-debugger-105778

Title Menu UI<br>
https://assetstore.unity.com/packages/tools/gui/full-menu-system-free-158919

UI Texture & Font<br>
https://assetstore.unity.com/packages/2d/gui/modern-gui-skin-19561

Map, Terrain<br>
https://assetstore.unity.com/packages/3d/environments/landscapes/rpg-poly-pack-lite-148410

Solder 모델 및 애니메이션<br>
https://assetstore.unity.com/packages/3d/characters/low-poly-soldiers-demo-73611

Robowarrior 모델 및 애니메이션<br>
https://assetstore.unity.com/packages/3d/characters/robots/sci-fi-warrior-pbr-hp-polyart-106154

Orc_Wolfrider 모델 및 애니메이션<br>
https://assetstore.unity.com/packages/3d/characters/humanoids/toon-rts-units-orcs-demo-101359

skybox texture<br>
https://assetstore.unity.com/packages/2d/textures-materials/sky/free-hdr-sky-61217

유니티 폴더 상에 임포트된 에셋은 조금 더 있지만 아예 사용하지 않아서 
출처에는 적지 않았습니다.
