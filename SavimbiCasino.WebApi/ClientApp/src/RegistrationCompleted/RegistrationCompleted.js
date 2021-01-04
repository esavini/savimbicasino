import React from "react";

import './style.css'

export default class RegistrationCompleted extends React.Component {

    render() {
        return (
            <div className="CompletedArea">
                <h1 className="CompletedTitle">Registrazione completata!</h1>
                <p>Utilizza il link di una stanza per accedere.</p>
            </div>
        );
    }
}