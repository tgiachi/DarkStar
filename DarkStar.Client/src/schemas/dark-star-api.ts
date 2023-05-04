/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AccountCreateRequestMessage {
  email?: string | null;
  password?: string | null;
}

export interface AccountCreateResponseMessage {
  success?: boolean;
  message?: string | null;
}

export interface AccountLoginRequestMessage {
  email?: string | null;
  password?: string | null;
}

export interface AccountLoginResponseMessage {
  success?: boolean;
}

export enum DarkStarMessageType {
  Ping = "Ping",
  Pong = "Pong",
  ServerVersionResponse = "ServerVersionResponse",
  ServerMotdResponse = "ServerMotdResponse",
  ServerNameResponse = "ServerNameResponse",
  ServerMessageResponse = "ServerMessageResponse",
  AccountLoginRequest = "AccountLoginRequest",
  AccountLoginResponse = "AccountLoginResponse",
  AccountCreateRequest = "AccountCreateRequest",
  AccountCreateResponse = "AccountCreateResponse",
  PlayerRacesRequest = "PlayerRacesRequest",
  PlayerRacesResponse = "PlayerRacesResponse",
  PlayerListRequest = "PlayerListRequest",
  PlayerListResponse = "PlayerListResponse",
  PlayerCreateRequest = "PlayerCreateRequest",
  PlayerCreateResponse = "PlayerCreateResponse",
  PlayerSelectRequest = "PlayerSelectRequest",
  PlayerSelectResponse = "PlayerSelectResponse",
  PlayerLoginRequest = "PlayerLoginRequest",
  PlayerLoginResponse = "PlayerLoginResponse",
  PlayerLogoutRequest = "PlayerLogoutRequest",
  PlayerLogoutResponse = "PlayerLogoutResponse",
  PlayerDataRequest = "PlayerDataRequest",
  PlayerDataResponse = "PlayerDataResponse",
  PlayerMoveRequest = "PlayerMoveRequest",
  PlayerMoveResponse = "PlayerMoveResponse",
  PlayerInventoryRequest = "PlayerInventoryRequest",
  PlayerInventoryResponse = "PlayerInventoryResponse",
  TileSetListRequest = "TileSetListRequest",
  TileSetListResponse = "TileSetListResponse",
  TileSetDownloadRequest = "TileSetDownloadRequest",
  TileSetDownloadResponse = "TileSetDownloadResponse",
  TileSetMapRequest = "TileSetMapRequest",
  TileSetMapResponse = "TileSetMapResponse",
  NpcAddedResponse = "NpcAddedResponse",
  NpcRemovedResponse = "NpcRemovedResponse",
  NpcMovedResponse = "NpcMovedResponse",
  ItemAddedResponse = "ItemAddedResponse",
  ItemRemovedResponse = "ItemRemovedResponse",
  ItemMovedResponse = "ItemMovedResponse",
  WorldGameObjectAddedResponse = "WorldGameObjectAddedResponse",
  WorldGameObjectRemovedResponse = "WorldGameObjectRemovedResponse",
  WorldGameObjectMovedResponse = "WorldGameObjectMovedResponse",
  PlayerGameObjectAddedResponse = "PlayerGameObjectAddedResponse",
  PlayerGameObjectRemovedResponse = "PlayerGameObjectRemovedResponse",
  PlayerGameObjectMovedResponse = "PlayerGameObjectMovedResponse",
  WorldMessageRequest = "WorldMessageRequest",
  WorldMessageResponse = "WorldMessageResponse",
  MapRequest = "MapRequest",
  MapResponse = "MapResponse",
}

export interface ItemAddedResponseMessage {
  mapId?: string | null;
  itemId?: string | null;
  name?: string | null;
  position?: PointPosition;
  /** @format int32 */
  tileType?: number;
}

export interface ItemMovedResponseMessage {
  mapId?: string | null;
  itemId?: string | null;
  position?: PointPosition;
}

export interface ItemRemovedResponseMessage {
  mapId?: string | null;
  itemId?: string | null;
  position?: PointPosition;
}

export interface LayerObjectSerialization {
  /** @format uuid */
  objectId?: string;
  /** @format int32 */
  tile?: number;
  type?: MapLayer;
  position?: PointPosition;
  properties?: Record<string, string>;
}

export interface MapEntityNetworkObject {
  /** @format int32 */
  id?: number;
  /** @format uuid */
  objectId?: string;
  /** @format int32 */
  tileType?: number;
  position?: PointPosition;
}

export enum MapLayer {
  Terrain = "Terrain",
  Objects = "Objects",
  Items = "Items",
  Creatures = "Creatures",
  Players = "Players",
  Effects = "Effects",
  Weather = "Weather",
}

export interface MapObjectSerialization {
  mapId?: string | null;
  mapType?: MapType;
  name?: string | null;
  /** @format int32 */
  width?: number;
  /** @format int32 */
  height?: number;
  layers?: LayerObjectSerialization[] | null;
}

export interface MapRequestMessage {
  mapId?: string | null;
}

export interface MapResponseMessage {
  mapId?: string | null;
  name?: string | null;
  mapType?: MapType;
  /** @format int32 */
  width?: number;
  /** @format int32 */
  height?: number;
  terrainsLayer?: MapEntityNetworkObject[] | null;
  gameObjectsLayer?: MapEntityNetworkObject[] | null;
  npcsLayer?: NamedMapEntityNetworkObject[] | null;
  itemsLayer?: NamedMapEntityNetworkObject[] | null;
  playersLayer?: NamedMapEntityNetworkObject[] | null;
}

export enum MapType {
  World = "World",
  Dungeon = "Dungeon",
  City = "City",
  Town = "Town",
  Village = "Village",
  Camp = "Camp",
  Ruins = "Ruins",
  Cave = "Cave",
  Forest = "Forest",
}

export enum MoveDirectionType {
  North = "North",
  South = "South",
  East = "East",
  West = "West",
  NorthEast = "NorthEast",
  NorthWest = "NorthWest",
  SouthEast = "SouthEast",
  SouthWest = "SouthWest",
}

export interface NamedMapEntityNetworkObject {
  /** @format int32 */
  id?: number;
  /** @format uuid */
  objectId?: string;
  /** @format int32 */
  tileType?: number;
  position?: PointPosition;
  name?: string | null;
}

export interface NetworkMessage {
  messageType?: DarkStarMessageType;
  /** @format byte */
  message?: string | null;
}

export interface NpcAddedResponseMessage {
  mapId?: string | null;
  npcId?: string | null;
  name?: string | null;
  position?: PointPosition;
  /** @format int32 */
  tileType?: number;
}

export interface NpcMovedResponseMessage {
  mapId?: string | null;
  npcId?: string | null;
  position?: PointPosition;
}

export interface NpcRemovedResponseMessage {
  mapId?: string | null;
  npcId?: string | null;
}

export interface PingMessageResponse {
  /** @format int64 */
  timeStamp?: number;
}

export interface PlayerCreateRequestMessage {
  name?: string | null;
  /** @format int32 */
  tileId?: number;
  /** @format uuid */
  raceId?: string;
  /** @format int32 */
  strength?: number;
  /** @format int32 */
  dexterity?: number;
  /** @format int32 */
  intelligence?: number;
  /** @format int32 */
  luck?: number;
}

export interface PlayerCreateResponseMessage {
  success?: boolean;
  /** @format uuid */
  playerId?: string;
}

export type PlayerDataRequestMessage = object;

export type PlayerDataResponseMessage = object;

export interface PlayerGameObjectAddedResponseMessage {
  mapId?: string | null;
  name?: string | null;
  id?: string | null;
  position?: PointPosition;
  /** @format int32 */
  tileId?: number;
}

export interface PlayerGameObjectMovedResponseMessage {
  mapId?: string | null;
  playerId?: string | null;
  position?: PointPosition;
}

export interface PlayerGameObjectRemovedResponseMessage {
  mapId?: string | null;
  id?: string | null;
}

export interface PlayerInventoryItem {
  /** @format uuid */
  itemId?: string;
  itemName?: string | null;
  itemDescription?: string | null;
  /** @format int32 */
  tileId?: number;
  /** @format int32 */
  quantity?: number;
}

export type PlayerInventoryRequestMessage = object;

export interface PlayerInventoryResponseMessage {
  items?: PlayerInventoryItem[] | null;
}

export type PlayerListRequestMessage = object;

export interface PlayerListResponseMessage {
  players?: PlayerObjectMessage[] | null;
}

export interface PlayerLoginRequestMessage {
  /** @format uuid */
  playerId?: string;
  playerName?: string | null;
}

export interface PlayerLoginResponseMessage {
  success?: boolean;
}

export type PlayerLogoutRequestMessage = object;

export interface PlayerLogoutResponseMessage {
  success?: boolean;
}

export interface PlayerMoveRequestMessage {
  direction?: MoveDirectionType;
}

export interface PlayerMoveResponseMessage {
  position?: PointPosition;
}

export interface PlayerObjectMessage {
  /** @format uuid */
  id?: string;
  name?: string | null;
  /** @format int32 */
  tile?: number;
  /** @format int32 */
  level?: number;
  race?: string | null;
}

export interface PlayerRaceObject {
  /** @format uuid */
  raceId?: string;
  /** @format int32 */
  tileId?: number;
  name?: string | null;
  /** @format int32 */
  strength?: number;
  /** @format int32 */
  dexterity?: number;
  /** @format int32 */
  intelligence?: number;
  /** @format int32 */
  luck?: number;
}

export type PlayerRacesRequestMessage = object;

export interface PlayerRacesResponseMessage {
  races?: PlayerRaceObject[] | null;
}

export interface PlayerSelectRequestMessage {
  /** @format uuid */
  playerId?: string;
}

export interface PlayerSelectResponseMessage {
  success?: boolean;
  message?: string | null;
}

export interface PointPosition {
  /** @format int32 */
  x?: number;
  /** @format int32 */
  y?: number;
}

export interface PongMessageResponse {
  /** @format int64 */
  timeStamp?: number;
}

export interface ServerMessageResponseMessage {
  message?: string | null;
  type?: ServerMessageType;
}

export enum ServerMessageType {
  Announcement = "Announcement",
  Information = "Information",
}

export interface ServerMotdResponseMessage {
  motd?: string | null;
}

export interface ServerNameResponseMessage {
  serverName?: string | null;
}

export interface ServerVersionResponseMessage {
  /** @format int32 */
  minor?: number;
  /** @format int32 */
  major?: number;
  /** @format int32 */
  build?: number;
}

export interface TileSetDownloadRequestMessage {
  tileName?: string | null;
}

export interface TileSetDownloadResponseMessage {
  tileSetName?: string | null;
  /** @format byte */
  tileSetData?: string | null;
}

export interface TileSetDto {
  /** @format uuid */
  id?: string;
  name?: string | null;
  /** @format int64 */
  fileSize?: number;
  /** @format int32 */
  tileWidth?: number;
  /** @format int32 */
  tileHeight?: number;
}

export interface TileSetEntryMessage {
  name?: string | null;
  /** @format int32 */
  tileHeight?: number;
  /** @format int32 */
  tileWidth?: number;
  /** @format int64 */
  fileSize?: number;
}

export type TileSetListRequestMessage = object;

export interface TileSetListResponseMessage {
  tileSets?: TileSetEntryMessage[] | null;
}

export interface TileSetMapEntry {
  /** @format int32 */
  tileId?: number;
  name?: string | null;
}

export interface TileSetMapRequestMessage {
  tileSetName?: string | null;
}

export interface TileSetMapResponseMessage {
  tileSetName?: string | null;
  tileSetMap?: TileSetMapEntry[] | null;
}

export interface WorldMessageRequestMessage {
  message?: string | null;
  sender?: string | null;
  messageType?: WorldMessageType;
}

export interface WorldMessageResponseMessage {
  message?: string | null;
  senderId?: string | null;
  senderName?: string | null;
  messageType?: WorldMessageType;
}

export enum WorldMessageType {
  Whisper = "Whisper",
  Normal = "Normal",
  Yell = "Yell",
}

export interface WorldObjectAddedResponseMessage {
  mapId?: string | null;
  itemId?: string | null;
  name?: string | null;
  position?: PointPosition;
  /** @format int32 */
  tileType?: number;
}

export interface WorldObjectMovedResponseMessage {
  mapId?: string | null;
  itemId?: string | null;
  position?: PointPosition;
}

export interface WorldObjectRemovedResponseMessage {
  mapId?: string | null;
  itemId?: string | null;
}

export namespace Api {
  /**
   * No description
   * @tags Script
   * @name ScriptsVariablesList
   * @request GET:/api/scripts/variables
   */
  export namespace ScriptsVariablesList {
    export type RequestParams = {};
    export type RequestQuery = {};
    export type RequestBody = never;
    export type RequestHeaders = {};
    export type ResponseBody = void;
  }
  /**
   * No description
   * @tags Script
   * @name ScriptsFunctionsList
   * @request GET:/api/scripts/functions
   */
  export namespace ScriptsFunctionsList {
    export type RequestParams = {};
    export type RequestQuery = {};
    export type RequestBody = never;
    export type RequestHeaders = {};
    export type ResponseBody = void;
  }
  /**
   * No description
   * @tags Tiles
   * @name TilesTilesetsList
   * @request GET:/api/tiles/tilesets
   */
  export namespace TilesTilesetsList {
    export type RequestParams = {};
    export type RequestQuery = {};
    export type RequestBody = never;
    export type RequestHeaders = {};
    export type ResponseBody = TileSetDto[];
  }
  /**
   * No description
   * @tags Tiles
   * @name TilesTilesetSourceDetail
   * @request GET:/api/tiles/tileset/source/{tileId}
   */
  export namespace TilesTilesetSourceDetail {
    export type RequestParams = {
      /** @format uuid */
      tileId: string;
    };
    export type RequestQuery = {};
    export type RequestBody = never;
    export type RequestHeaders = {};
    export type ResponseBody = void;
  }
  /**
   * No description
   * @tags Version
   * @name VersionVersionList
   * @request GET:/api/version/version
   */
  export namespace VersionVersionList {
    export type RequestParams = {};
    export type RequestQuery = {};
    export type RequestBody = never;
    export type RequestHeaders = {};
    export type ResponseBody = string;
  }
}
