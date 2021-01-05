import React from "react";

import './style.css'
import {Alert, Button, Form, FormText, Spinner, Table} from "react-bootstrap";
import {withRouter} from "react-router-dom"
import UserContext, {UserProvider} from "../UserContext";

class Login extends React.Component {

    state = {
        isLoading: false
        , errors: []
        , form: {
            username: ''
            , password: ''
        }
        , from: null
    }

    constructor(params) {
        super(params);

        this.onInputChange = this.onInputChange.bind(this)
        this.onSubmit = this.onSubmit.bind(this)
    }

    onInputChange(e) {
        const target = e.target

        let newState = {...this.state}
        newState.form[target.name] = target.value

        this.setState(newState)
    }

    onSubmit(e) {
        e.preventDefault()

        if (this.state.isLoading)
            return

        let from = this.props.location.state.from
        let history = this.props.history

        this.setState({
            ...this.state,
            isLoading: true
            , errors: []
        })

        fetch('/v1/Player/Login', {
            method: 'post'
            , body: JSON.stringify(this.state.form)
            , headers: {
                'Accept': 'application/json'
                , 'Content-Type': 'application/json'
            }
        }).then(response => {

            if (response.status === 201 || response.status === 200) {
                return response.json();
            }

            let newState = {
                ...this.state,
                errors: []
            }

            if (response.status === 404)
                newState.errors.push("Nome utente errato!")

            if (response.status === 401)
                newState.errors.push("Password errata!")

            newState.isLoading = false

            this.setState(newState)

        })
            .then(data => {
                if (data === undefined) {
                    return
                }
                
                this.context.user.token = data.token
                this.context.isLogged = true
                history.push(from)
            })
            .catch(err => {
                this.state.errors.push(err.message)
                this.state.isLoading = false
            })
    }

    render() {
        return (
            <div className="LoginArea">
                <h1 className="LoginTitle">Login</h1>
                {
                    this.state.errors.map((text, index) => {
                        return <Alert key={index} variant="danger">
                            {text}
                        </Alert>
                    })
                }
                <Form onSubmit={this.onSubmit}>
                    <Form.Group controlId="formLogin">
                        <Form.Label>Nome</Form.Label>
                        <Form.Control type="text"
                                      required
                                      name="username"
                                      placeholder="Nome"
                                      minLength="3"
                                      maxLength="10"
                                      disabled={this.state.isLoading}
                                      onChange={this.onInputChange}
                                      value={this.state.form.username}/>
                    </Form.Group>

                    <Form.Group controlId="formLoginPassword">
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password"
                                      required
                                      name="password"
                                      placeholder="Password"
                                      minLength="6"
                                      onChange={this.onInputChange}
                                      value={this.state.form.password}
                                      disabled={this.state.isLoading}/>
                    </Form.Group>
                    <Button variant="primary" type="submit" disabled={this.state.isLoading}>
                        <span style={{display: this.state.isLoading ? 'none' : 'block'}}>Accedi</span>
                        <Spinner animation="border"
                                 role="status"
                                 style={{display: this.state.isLoading ? 'block' : 'none'}}>
                            <span className="sr-only">Caricamento...</span>
                        </Spinner>
                    </Button>

                </Form>
            </div>
        );
    }
}

Login.contextType = UserContext

export default withRouter(Login)