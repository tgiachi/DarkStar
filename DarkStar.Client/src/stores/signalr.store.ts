import {RootStore} from "./root.store.ts";
import {makeAutoObservable} from "mobx";

export class SignalrStore {

    rootStore: RootStore

    constructor(rootStore: RootStore) {
        makeAutoObservable(this)
        this.rootStore = rootStore;
        console.log("SignalR created")
    }
}
