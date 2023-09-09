import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import { Layout } from './components/Layout';
import './custom.css';
import AuthorizedRoute from "./AuthorizedRoute";
import {Home} from "./components/Home";
import {Counter} from "./components/Counter";
import {FetchData} from "./components/FetchData";
import Account, {Login, Register} from "./components/account";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Routes>
            <Route exact path='/' element={<Home/>} />

            <Route path='/counter' element={<Counter/>} />
            <Route path='/fetch-data' element={<AuthorizedRoute/>}>
                <Route  element={<FetchData/>}/>
            </Route>
            <Route path='/Account'>
                <Route path='Login' isForAuthenticated={false} element={<Login/>} />
                <Route path='Register' isForAuthenticated={false} element={<Register/>} />
            </Route>
        </Routes>
      </Layout>
    );
  }
}
