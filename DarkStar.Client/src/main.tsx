import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import {createBrowserRouter, RouterProvider} from "react-router-dom"
import {initialValues, StoreContext} from "./stores/context/store.context.tsx";

const router = createBrowserRouter([
    {
        path: '/',
        element: <App/>
    }
])

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <StoreContext.Provider value={initialValues}>
            <RouterProvider router={router}/>
        </StoreContext.Provider>
    </React.StrictMode>,
)
