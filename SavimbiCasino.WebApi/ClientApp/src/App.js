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
import Register from "./Register";
import Login from "./Login";
import RegistrationCompleted from "./RegistrationCompleted";

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
                    <Route path="/registrationCompleted">
                        <RegistrationCompleted/>
                    </Route>
                    <Route path="/login">
                        <Login/>
                    </Route>
                    <Route path="/dealer/admin/:roomId">
                        <DealerAdmin/>
                    </Route>
                    <Route path="/dealer/:roomId">
                        <Dealer/>
                    </Route>
                    <Route path="/" component={Register}/>
                </Switch>
            </Router>
        )
    }
}
