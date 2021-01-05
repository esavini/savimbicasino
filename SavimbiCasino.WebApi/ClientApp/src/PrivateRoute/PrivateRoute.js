import UserContext from "../UserContext";

import React, {useEffect} from "react";
import {Route, Redirect} from "react-router-dom"

export default class PrivateRoute extends React.Component {
    render() {
        return (
            <Route {...this} render={({ location }) => (
                <UserContext.Consumer>
                    {({isLogged}) => (
                        isLogged ? (
                            <this.props.component {...this.props} />
                        ) : (
                            <Redirect
                                to={{
                                    pathname: "/login"
                                    ,state: {
                                        from: location
                                    }
                                }}
                            />
                        )

                    )}
                </UserContext.Consumer>
            )}/>
        )
    }

}