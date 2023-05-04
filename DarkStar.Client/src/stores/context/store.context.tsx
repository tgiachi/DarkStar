import { createContext, useContext } from "react";
import {RootStore} from "../root.store.ts";

interface IStoresContext {

    rootStore: RootStore
}

const initialValues : IStoresContext = {
    rootStore: new RootStore()
}

const StoreContext = createContext<IStoresContext>(initialValues);

const useStore = () => {
    const store = useContext(StoreContext);
    if (!store) {
        throw new Error("useStore must be used within a StoreProvider");
    }
    return store;
}

// eslint-disable-next-line react-refresh/only-export-components
export { StoreContext, initialValues, useStore };
