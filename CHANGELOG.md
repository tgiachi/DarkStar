# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="0.8.0"></a>
## [0.8.0](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.8.0) (2023-5-8)

### ✨ Features

* I've come to a conclusion, I hate typescript, I hate javascript. So the client will be done in Avalonia! 😂 ([2bc4208](https://www.github.com/tgiachi/DarkSun/commit/2bc4208d8860f5d633b5a6bf298dae8c92565566))
* **client:** add initial Avalonia UI project files and setup dependency injection and logging ([abd2165](https://www.github.com/tgiachi/DarkSun/commit/abd21651326cbd88ae59424c479b36fe2470ea28))
* **client:** add splash screen window with DarkStar logo and progress bar ([b8b010e](https://www.github.com/tgiachi/DarkSun/commit/b8b010eba95fc5a4b93e7aab718f08a324e6b017))
* **client:** add support for TileSetListRequestMessage and AccountLoginResponseMessage ([194be72](https://www.github.com/tgiachi/DarkSun/commit/194be7203b1c91a1703b14096cbb79c5721dcfa9))
* **PageViewControl:** add ContentPresenter to PageViewControl ([7091612](https://www.github.com/tgiachi/DarkSun/commit/709161252652c07b2c277d790ab4dc92c533203a))
* **PageViewControl:** add PageViewControl to MainWindow ([c861eff](https://www.github.com/tgiachi/DarkSun/commit/c861effda59220237e1525b60b87d138b9b58222))

### Other

* 🎨 style(.gitignore): add DarkStar.Client/.idea to .gitignore ([1abf55c](https://www.github.com/tgiachi/DarkSun/commit/1abf55ccae5f9eba09a1ac6936c6913a68abfa4d))
* 🐛 fix(LoginPageViewModel.cs): change network client address and port to localhost:5000 ([4c9090e](https://www.github.com/tgiachi/DarkSun/commit/4c9090e726617bfdb64f48375ad069c202a37a32))
* 🔥 chore(DarkStar.Api, DarkStar.Client): remove unused code and files ([e6fa2b8](https://www.github.com/tgiachi/DarkSun/commit/e6fa2b8337013abeed2ff04262fe64e911db964c))

<a name="0.7.0"></a>
## [0.7.0](https://www.github.com/tgiachi/DarkStar/releases/tag/v0.7.0) (2023-5-4)

### ✨ Features

* added test client ([1abaf8d](https://www.github.com/tgiachi/DarkStar/commit/1abaf8d17d4d92cad9d51253ccc43350206721ca))

### Other

* 🐛 fix(AssemblyUtils.cs): make AssemblyUtils class static to avoid instantiation ([55c888a](https://www.github.com/tgiachi/DarkStar/commit/55c888ab63684fde5d648ba1e836e10d71d56a43))
* 🔥 chore(DarkStar.Engine.Runner.csproj): comment out ServerGarbageCollection property ([3b29bb1](https://www.github.com/tgiachi/DarkStar/commit/3b29bb1c1d49e4c407b66bfcce513b1f5e23df65))
* 🔥 chore(DarkStar.Engine.Runner.csproj): remove empty lines ([71acf67](https://www.github.com/tgiachi/DarkStar/commit/71acf679da57d102a56ed8cebedc8c1a3aa23fd1))

<a name="0.6.3"></a>
## [0.6.3](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.6.3) (2023-5-4)

### Other

* ✨ feat(SeedService.cs): add AttachTextContentToItem method ([a485fca](https://www.github.com/tgiachi/DarkSun/commit/a485fca01bfa9f40ca90300e0ee8ebaf8e7bb017))
* 🎉 feat(dependabot.yml): add Dependabot configuration file for NuGet package ecosystem ([2979485](https://www.github.com/tgiachi/DarkSun/commit/2979485df48e92ad7916ab68028475942995e9bc))
* 🎨 style(WorldService.cs): refactor object creation to use object initializer syntax ([9d2fc0b](https://www.github.com/tgiachi/DarkSun/commit/9d2fc0b7266ba7fe61c4a576af4a087b1dbe5cf7))
* 🔧 chore: remove unused HttpServerConfig from EngineConfig ([6f86534](https://www.github.com/tgiachi/DarkSun/commit/6f86534bfab0b7354ee9a5b318ec2965ec200362))
* 🚀 chore(DarkStar.Client): add @microsoft/signalr and mobx-react dependencies ([0a11fd1](https://www.github.com/tgiachi/DarkSun/commit/0a11fd107c7ae9259010ddcf042a6307609feadd))
* 🚀 feat(EngineConfig.cs): add ExperimentalConfig section to EngineConfig ([2d62566](https://www.github.com/tgiachi/DarkSun/commit/2d6256638b0391950e0005bed92ed8d03c49c063))

<a name="0.6.2"></a>
## [0.6.2](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.6.2) (2023-5-3)

### Other

* 🎉 feat(nuget.yml): add GitHub workflow to publish Void libraries on NuGet ([b29552f](https://www.github.com/tgiachi/DarkSun/commit/b29552f08d0c443d17ff04fdae85046795f1d31e))
* 🎨 style(csproj): add assembly info generation and package metadata to csproj files ([3d1b151](https://www.github.com/tgiachi/DarkSun/commit/3d1b1519b78bb18458a5ff2a6b46638e14745abc))
* 🐛 fix(BinarySerialization.cs): change BinarySerialization class to static ([6b61bb6](https://www.github.com/tgiachi/DarkSun/commit/6b61bb64e8937171f5db587b59f7789e1f182360))
* 🐛 fix(ItemService.cs): add missing await to AddScriptableGameObjectAction method call ([a2dd8ad](https://www.github.com/tgiachi/DarkSun/commit/a2dd8adc00110eade4a7d0995623a1918a4a80c2))
* 🐛 fix(nuget.yml): add newline at end of file ([2b2ff9f](https://www.github.com/tgiachi/DarkSun/commit/2b2ff9f1c8c351d87cbf049e3ed8fe23e2a1e427))
* 📝 docs(nuget.yml): update workflow name to reflect project name change ([e3bdd02](https://www.github.com/tgiachi/DarkSun/commit/e3bdd02de25529078815faead3810144483ddf05))
* 🚀 chore(dependabot.yml): add Dependabot configuration file ([d16f48d](https://www.github.com/tgiachi/DarkSun/commit/d16f48d08e9c6e34b72a38c888dd9fa8c87ed85b))
* Create dependabot.yml ([7bae235](https://www.github.com/tgiachi/DarkSun/commit/7bae235d274f050ea4590dd19e0f143a55091a6a))
* Merge branch 'dev' of github.com:tgiachi/DarkSun into dev ([6075519](https://www.github.com/tgiachi/DarkSun/commit/6075519a7bf4294a69934219f9c84934759f8953))
* Merge branch 'main' into dev ([5c2c718](https://www.github.com/tgiachi/DarkSun/commit/5c2c718218ecad1fc10e7ecaea18e7ed3e47825e))
* Merge pull request #17 from tgiachi/dev ([1ae1015](https://www.github.com/tgiachi/DarkSun/commit/1ae1015e2cbe5ffafcd231e2d671b83e387fa1bb))
* Merge pull request #18 from tgiachi/dev ([20a8b1b](https://www.github.com/tgiachi/DarkSun/commit/20a8b1b8947d2d4c953428097b28dfe46930e403))
* Merge pull request #19 from tgiachi/dev ([1c88e84](https://www.github.com/tgiachi/DarkSun/commit/1c88e849af5138461dc3b389ef97365435fc97d8))
* Merge pull request #2 from tgiachi/dev ([0feba20](https://www.github.com/tgiachi/DarkSun/commit/0feba20c8f104f0dc31bff84c204e0c6ec25c3d8))
* Merge pull request #20 from tgiachi/dev ([45e1122](https://www.github.com/tgiachi/DarkSun/commit/45e112248b98cbcf4630f229dfd59f1b0b8046fc))
* Merge pull request #21 from tgiachi/dev ([7488c39](https://www.github.com/tgiachi/DarkSun/commit/7488c394c183e84b5c06c490ba840bd1d78c0ace))
* Merge pull request #22 from tgiachi/dev ([0dafb1c](https://www.github.com/tgiachi/DarkSun/commit/0dafb1c63bd3beb08ab9cbbee4cf91c8e85cf024))
* Merge pull request #23 from tgiachi/dev ([3542dcc](https://www.github.com/tgiachi/DarkSun/commit/3542dccd0f61798ec39ceb75b4756d92a063c771))
* Merge pull request #24 from tgiachi/dev ([2219ee3](https://www.github.com/tgiachi/DarkSun/commit/2219ee3dca5aeb37c3f8726538114309af87d5a4))
* Merge pull request #26 from tgiachi/dev ([80115f1](https://www.github.com/tgiachi/DarkSun/commit/80115f1ab2e136b5d8753b7abbd0f6df4b7f9be1))
* Merge pull request #27 from tgiachi/dev ([54b37a7](https://www.github.com/tgiachi/DarkSun/commit/54b37a7a8448471129ab52d2751b797328de871f))
* Merge pull request #28 from tgiachi/dev ([bcbec47](https://www.github.com/tgiachi/DarkSun/commit/bcbec47ccc26504b88b5569d69a9555abcf6d2d9))
* Merge pull request #3 from tgiachi/dependabot/nuget/Microsoft.VisualStudio.Threading.Analyzers-17.5.22 ([30bead7](https://www.github.com/tgiachi/DarkSun/commit/30bead7bf86c3bfd2484c716f1e61f5e34232ebc))
* Merge pull request #30 from tgiachi/dev ([53e1c75](https://www.github.com/tgiachi/DarkSun/commit/53e1c759a6f6f7961ee12fe8b6c4c7c1faf173cf))
* Merge pull request #31 from tgiachi/dev ([21d7e9f](https://www.github.com/tgiachi/DarkSun/commit/21d7e9fefc123c4b2e22f01c20d8a8b61e6ef3c3))
* Merge pull request #32 from tgiachi/dev ([f2a3b00](https://www.github.com/tgiachi/DarkSun/commit/f2a3b00f2dc864c9571ecbf8a28d3199ff229fa0))
* Merge pull request #33 from tgiachi/dev ([e50c49d](https://www.github.com/tgiachi/DarkSun/commit/e50c49dea886bbb87a6b81d8c8eea32edd9288dc))
* Merge pull request #34 from tgiachi/dev ([7191caa](https://www.github.com/tgiachi/DarkSun/commit/7191caab893aa02c86caea57aceb73d0255e8c7d))
* Merge pull request #35 from tgiachi/dev ([3c8982b](https://www.github.com/tgiachi/DarkSun/commit/3c8982baab63caf25ef724b3a8953ff6a4fff6de))
* Merge pull request #36 from tgiachi/dependabot/nuget/Nerdbank.GitVersioning-3.6.128 ([1e73648](https://www.github.com/tgiachi/DarkSun/commit/1e736483aedccd2322e787b7a716bfc42be0ca04))
* Merge pull request #38 from tgiachi/dev ([ab674a7](https://www.github.com/tgiachi/DarkSun/commit/ab674a7f0fd5bc7013de64cfdf52fddca6337cb0))
* Merge pull request #39 from tgiachi/dev ([b2e2204](https://www.github.com/tgiachi/DarkSun/commit/b2e2204d456e06e9eae0af2e741c0fe98be9545d))
* **deps:** bump Microsoft.VisualStudio.Threading.Analyzers ([88aa9c9](https://www.github.com/tgiachi/DarkSun/commit/88aa9c9f4433d185eb65ccfc95e5964f4b09622d))
* **deps:** bump Nerdbank.GitVersioning from 3.5.119 to 3.6.128 ([23976e5](https://www.github.com/tgiachi/DarkSun/commit/23976e54a7b6c408dca8e6c40e1b696b52238991))

<a name="0.6.1"></a>
## [0.6.1](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.6.1) (2023-5-3)

### Other

* **AiService.cs:** rename AddScriptNpc method to AddScriptNpcAsync ([37719e4](https://www.github.com/tgiachi/DarkSun/commit/37719e4299cff35362b8c5c1f78ece36d238cd5b))
* **all:** applied standard code format ([10eb144](https://www.github.com/tgiachi/DarkSun/commit/10eb14405d39f9ea8ee346596a51f6f7fa091110))

<a name="0.6.0"></a>
## [0.6.0](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.6.0) (2023-5-2)

### ✨ Features

* add support for item types and item category types ([1b52abf](https://www.github.com/tgiachi/DarkSun/commit/1b52abf9981b9ec040758b7f817dfe53c3e3e940))
* added base project for client ([34ba796](https://www.github.com/tgiachi/DarkSun/commit/34ba796d166428f187ee5ecf6dd63095e296e59c))
* **ai:** add support for adding AI scripts by NPC type and name ([8d687ff](https://www.github.com/tgiachi/DarkSun/commit/8d687ff96c05b5cda5abed04b1e193b5693e26c3))
* **SeedService.cs:** add method to add text content seed ([62694d0](https://www.github.com/tgiachi/DarkSun/commit/62694d0517a01c2f9db598dad016a05c8268d4b7))

### Other

* remove unused TileSetMapSerializable class ([1cc4bc3](https://www.github.com/tgiachi/DarkSun/commit/1cc4bc370f75c6c3ae17f68e8a503596b0f4a663))

<a name="0.5.0"></a>
## [0.5.0](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.5.0) (2023-4-28)

### ✨ Features

* **network:** add SignalR network client implementation ([e2239ee](https://www.github.com/tgiachi/DarkSun/commit/e2239ee98a0c9f00f52ac1fb86630e1c01f0c5f6))

### Other

* ✨ feat(typesScriptModule.cs): add support for adding tiles, game object seeds and get functions ([16c490a](https://www.github.com/tgiachi/DarkSun/commit/16c490ad6e24d6ab9e86864293a462eef1fe6d86))
* 🔥 chore(DarkStar.Network.csproj): remove unused SignalR packages ([c0984af](https://www.github.com/tgiachi/DarkSun/commit/c0984af682d4ede448ffa1d4604f5fbdba0065ff))
* 🔧 chore(BaseNetworkConnectionHandler.cs, PlayerSession.cs, PlayerLoggedEvent.cs, INetworkConnectionHandler.cs, IPlayerService.cs, IWorldService.cs, PlayerGameObject.cs, BaseNetworkServerMessageListener.cs, WebModuleLoaderExtension.cs, Program.cs, PlayerMoveAction.cs, DefaultConnectionHandler.cs, DarkSunEngine.cs, AccountCreationServerMessageListener.cs, AccountLoginServerMessageListener.cs, PlayerCreationServerMessageListener.cs, PlayerListServerMessageListener.cs): change Guid sessionId to string sessionId in all relevant files ([f4db356](https://www.github.com/tgiachi/DarkSun/commit/f4db3565abf870b96beea9b5f7edab64eee6f5a8))

<a name="0.4.0"></a>
## [0.4.0](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.4.0) (2023-4-27)

### ✨ Features

* **types:** add support for NpcType and NpcSubType ([b9a8a88](https://www.github.com/tgiachi/DarkSun/commit/b9a8a886e8196f4f303c33bf13b2753afa757785))

### Other

* ✨ feat(BlueprintGenerationMapContext.cs): add support for adding NPCs to the map ([a1a0d64](https://www.github.com/tgiachi/DarkSun/commit/a1a0d64084d507dbf7c57bcaecf6994726f1ef51))
* 🐛 fix(MushroomFinderAi.cs): fix null reference exception when searching for mushrooms ([a8132bb](https://www.github.com/tgiachi/DarkSun/commit/a8132bb835332781bef15669f2ae85d57d78043a))
* 🔧 fix(server.ts): change port variable case from lowercase port to uppercase PORT ([a2ac148](https://www.github.com/tgiachi/DarkSun/commit/a2ac1485d9c12a156d0f69314cd0b811f7a0d289))
* 🔧 refactor(GameObjectType.cs): change GameObjectType from enum to struct with Id and Name properties ([03d563b](https://www.github.com/tgiachi/DarkSun/commit/03d563b9e0177d332cd86eb9606c1dbfb9f689ce))
* change TileId type from int to uint in multiple files ([abdfcd9](https://www.github.com/tgiachi/DarkSun/commit/abdfcd96893808194f4f9d789eb9cd33293129c6))
* refactor tiles and network ([038c4ea](https://www.github.com/tgiachi/DarkSun/commit/038c4ea3ac33c64c1a611661ad837cb951f777ad))
* remove unused code ([1c03a30](https://www.github.com/tgiachi/DarkSun/commit/1c03a3092dbc7e2b61c5a2ca853ee7893f0ad71f))
* **ai:** change NpcType and NpcSubType to use string instead of ushort ([482819f](https://www.github.com/tgiachi/DarkSun/commit/482819f4f6173a3e85bcbe7b2e89dc3ef19ecc61))
* **MessageHub.cs:** remove unnecessary whitespace and add blank line ([c3fe84a](https://www.github.com/tgiachi/DarkSun/commit/c3fe84a18bae7db8cfedc873527fb5f45ebcdef4))
* **PlayerService.cs:** simplify GetPlayerByIdAsync method with null-coalescing operator ([6c29ba9](https://www.github.com/tgiachi/DarkSun/commit/6c29ba91d2a17e5946eb4b1d13918eba6e40c5a7))

<a name="0.3.0"></a>
## [0.3.0](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.3.0) (2023-4-21)

### ✨ Features

* **ai:** add support for sending world messages in CatAi and MushroomFinderAi ([1af8e34](https://www.github.com/tgiachi/DarkSun/commit/1af8e3409f8b1014669497d853fcde595503d581))

<a name="0.2.0"></a>
## [0.2.0](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.2.0) (2023-4-21)

### ✨ Features

* add support for removing scheduled world objects ([02a8839](https://www.github.com/tgiachi/DarkSun/commit/02a8839129ccc60a00fe5c8ee34c026e81ab2762))

<a name="0.1.12"></a>
## [0.1.12](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.12) (2023-4-20)

### Other

* 🐛 fix(BaseAiBehaviourExecutor.cs): change CalculatePath to CalculateAStarPath ([8b18fd2](https://www.github.com/tgiachi/DarkSun/commit/8b18fd213758ca09550eda47c6ad5946423c3305))

<a name="0.1.11"></a>
## [0.1.11](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.11) (2023-4-20)

### Other

* 🐛 fix(BaseEnumConverter.cs): fix enum value search with wildcard ([8113bbd](https://www.github.com/tgiachi/DarkSun/commit/8113bbd0de0c2355b7971ee1416e67579395d50c))

<a name="0.1.10"></a>
## [0.1.10](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.10) (2023-4-20)

### Other

* 🔨 refactor(PointConverterEx.cs): rename PlayerMoveDirectionType to MoveDirectionType ([e88d597](https://www.github.com/tgiachi/DarkSun/commit/e88d597e1029665b2fe1c0baf6165e9efa50c7d7))

<a name="0.1.9"></a>
## [0.1.9](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.9) (2023-4-20)

### Other

* namespace filescoped ([97d2e07](https://www.github.com/tgiachi/DarkSun/commit/97d2e072c713706b683bb3fc441bc721d126aca8))

<a name="0.1.8"></a>
## [0.1.8](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.8) (2023-4-20)

### Other

* ✨ feat(.versionize): add configuration for changelogAll and changelog sections ([db61fef](https://www.github.com/tgiachi/DarkSun/commit/db61fef4588280fd0b8d5f0bb74f7e57360eced7))

<a name="0.1.7"></a>
## [0.1.7](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.7) (2023-4-20)

### Bug Fixes

* message parser token ([bc8224a](https://www.github.com/tgiachi/DarkSun/commit/bc8224ab6591b719dc3b5419ca5f7a19a266c34d))

<a name="0.1.6"></a>
## [0.1.6](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.6) (2023-4-19)

<a name="0.1.5"></a>
## [0.1.5](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.5) (2023-4-19)

<a name="0.1.4"></a>
## [0.1.4](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.4) (2023-4-19)

<a name="0.1.3"></a>
## [0.1.3](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.3) (2023-4-18)

<a name="0.1.2"></a>
## [0.1.2](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.2) (2023-4-18)

<a name="0.1.1"></a>
## [0.1.1](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.1) (2023-4-17)

<a name="0.1.0"></a>
## [0.1.0](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.1.0) (2023-4-17)

### Features

* add new attributes and base classes for command, network, object, and seed actions ([9418148](https://www.github.com/tgiachi/DarkSun/commit/941814818728e6ccd4f08bb9a776a5f37a92f007))
* add new attributes for game object and item actions ([e50c928](https://www.github.com/tgiachi/DarkSun/commit/e50c928c38b7a914c2f3c19380c236b1c6cb1181))
* **commands:** add command action attribute and base command action executor ([ac953c5](https://www.github.com/tgiachi/DarkSun/commit/ac953c5738eb3a03658c6cddde25dec99f0a13ce))
* **TileType.cs:** add new tile types for walls, items, food, props, and animals ([c37a9cc](https://www.github.com/tgiachi/DarkSun/commit/c37a9cc60f63e143aa2333d64920d36e4f2e1e88))
* **TileType.cs:** add new tile types for walls, items, food, props, and animals ([3b01cb6](https://www.github.com/tgiachi/DarkSun/commit/3b01cb6432d9d9a875dac8b1ba14bd8eae9e3759))

### Bug Fixes

* **ci.yml:** change DarkSun to DarkStar in xmllint command ([1852601](https://www.github.com/tgiachi/DarkSun/commit/18526018d01ce1e40992d9a83ea09a24bae5db2a))
* **ci.yml:** change Docker image tag from 'tgiachi/DarkStar.server:latest' to 'tgiachi/darkstar.server:latest' ([37021e3](https://www.github.com/tgiachi/DarkSun/commit/37021e3f01958996e2993c9f42707ececca49477))
* **Program.cs:** change DARKSUN to DARKSTAR in root directory variable name and default value ([c21ac7d](https://www.github.com/tgiachi/DarkSun/commit/c21ac7d5ead1f1d63281d5d0fa0077a7a3c824a4))
* **README.md:** change Dark Sun to Dark Star in the game description ([76e189a](https://www.github.com/tgiachi/DarkSun/commit/76e189a6af6469638d6292c12d85b6b37216d3af))

<a name="0.0.1"></a>
## [0.0.1](https://www.github.com/tgiachi/DarkSun/releases/tag/v0.0.1) (2023-4-13)

### Features

* add base entity and interface for database entities ([1644b18](https://www.github.com/tgiachi/DarkSun/commit/1644b1860fad4b9129a57d030c5a6fa03b068757))
* add DarkSunEngineServiceAttribute to mark services for DarkSunEngine ([d238f55](https://www.github.com/tgiachi/DarkSun/commit/d238f55a2e778883747ed124e599c77d5ed87680))
* add IDatabaseService to IDarkSunEngine interface ([5386114](https://www.github.com/tgiachi/DarkSun/commit/5386114d68fc09844ef851823f60d6f2bc609f0f))
* add new class PlayerSession to manage player sessions in the engine ([796298c](https://www.github.com/tgiachi/DarkSun/commit/796298c41be6bd7d02bad293a570f53b1421b9e8))
* add new enums and entities ([e1dbfc2](https://www.github.com/tgiachi/DarkSun/commit/e1dbfc2d324b63e58d768f49e27a4bf6bc710f6b))
* add new interfaces and entities for NPCs and players ([7af0c03](https://www.github.com/tgiachi/DarkSun/commit/7af0c03bd7027365950d51877d4d3e8b05c3c403))
* add SharedAssemblyInfo.cs, SharedGlobalUsings.cs, and stylecop.json files ([ae00f97](https://www.github.com/tgiachi/DarkSun/commit/ae00f97c40784b4cdab124c9ea35b7a1a4b2fb58))
* **DarkSun.Api.Engine:** add engine project and related classes ([57450b0](https://www.github.com/tgiachi/DarkSun/commit/57450b0e0bc114972bd890dbba51b6322b5bf1de))
* **DarkSun.Api.Engine.csproj:** add GoRogue package reference ([3343557](https://www.github.com/tgiachi/DarkSun/commit/33435575180ef6516d7e07c0f8ce486edc317f78))
* **DarkSun.Api.Engine.csproj:** add Redbus package reference ([f75c2dd](https://www.github.com/tgiachi/DarkSun/commit/f75c2dda2b352e4c56b462e42c4fb7e843248478))
* **DarkSun.Network:** add IDarkSunNetworkClient interface ([e19aa97](https://www.github.com/tgiachi/DarkSun/commit/e19aa97e090b8db4b1b1033aa07322a8b722f53f))
* **MessagePackMessageBuilder.cs:** add support for message separators ([6a1dcea](https://www.github.com/tgiachi/DarkSun/commit/6a1dceaedabd66d807f161774db1547ba71ef7af))

### Bug Fixes

* **MessagePackNetworkServer.cs:** add null-forgiving operator to OnClientConnected invocation ([3221f83](https://www.github.com/tgiachi/DarkSun/commit/3221f83d594c7f29a70ec307ea8ad5d0d986fbdd))

