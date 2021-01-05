import Dealer from "./Dealer";
import Admin from "./Admin";
import PlayerConsole from "./PlayerConsole";
import DealerAdmin from "./DealerAdmin";
import Register from "./Register";
import Login from "./Login";
import RegistrationCompleted from "./RegistrationCompleted";
import PrivateRoute from "./PrivateRoute";

import React from "react";
import {
    BrowserRouter as Router,
    Switch,
    Route
} from "react-router-dom";

import './App.css';

export default class App extends React.Component {
    render() {
        return (
            <Router>
                <Switch>
                    <PrivateRoute exact path="/room/:roomId" component={PlayerConsole}/>
                    <Route path="/admin">
                        <Admin/>
                    </Route>
                    <Route path="/registrationCompleted">
                        <RegistrationCompleted/>
                    </Route>
                    <Route path="/login">
                        <Login/>
                    </Route>
                    <Route exact path="/dealer/admin/:roomId">
                        <DealerAdmin/>
                    </Route>
                    <Route exact path="/dealer/:roomId">
                        <Dealer/>
                    </Route>
                    <Route path="/" component={Register}/>
                </Switch>
            </Router>
        )
    }
}
