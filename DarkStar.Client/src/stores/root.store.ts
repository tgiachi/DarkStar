import {SignalrStore} from "./signalr.store.ts";


export class RootStore {

    signalRStore: SignalrStore

    constructor() {
        console.log("RootStore Created")
        this.signalRStore = new SignalrStore(this);
    }
}
