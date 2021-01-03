import Dealer from "./Dealer";
import Admin from "./Admin";
import PlayerConsole from "./PlayerConsole";
import DealerAdmin from "./DealerAdmin";

import React from "react";
import {
    BrowserRouter as Router,
    Switch,
    Route,
} from "react-router-dom";

import './App.css';

export default class App extends React.Component {
    render() {
        return (
            <Router>
                <Switch>
                    <Route path="/room/:roomName">
                        <PlayerConsole/>
                    </Route>
                    <Route path="/admin">
                        <Admin/>
                    </Route>
                    <Route path="/dealer/admin/:roomName">
                        <DealerAdmin/>
                    </Route>
                    <Route path="/dealer/:roomName">
                        <Dealer/>
                    </Route>
                    <Route path="/">
                        <Admin/>
                    </Route>
                </Switch>
            </Router>
        )
    }
}
