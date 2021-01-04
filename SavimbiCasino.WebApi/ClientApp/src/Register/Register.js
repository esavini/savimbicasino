import React from "react";

import './style.css'
import {Alert, Button, Form, FormText, Spinner, Table} from "react-bootstrap";
import {withRouter} from "react-router-dom"

class Register extends React.Component {

    state = {
        isLoading: false
        , errors: []
        , form: {
            username: ''
            ,password: ''
        }
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
        
        let history = this.props.history
        
        if(this.state.isLoading)
            return
        
        this.setState({
            ...this.state
            , isLoading: true
        })

        fetch('/v1/Player/Register', {
            method: 'post'
            , body: JSON.stringify(this.state.form)
            , headers: {
                'Accept': 'application/json'
                ,'Content-Type': 'application/json'
            }
        }).then(response => {
            history.push("/registrationCompleted")
        }).catch(err => {
            let newState = {
                ...this.state
                , isLoading: false
                ,errors: [err]
            }
            
            this.setState(newState)
        })
    }

    render() {
        return (
            <div className="RegisterArea">
                <h1 className="RegisterTitle">Registrazione</h1>
                {
                    this.state.errors.map((text, index) => {
                        return <Alert key={index} variant="danger">
                            {text}
                        </Alert>
                    })
                }
                <Form onSubmit={this.onSubmit}>
                    <Form.Group controlId="formRegister">
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
                        <Form.Text className="text-muted">
                            Da 3 a 10 caratteri, sono ammessi anche gli emoji.
                        </Form.Text>
                    </Form.Group>

                    <Form.Group controlId="formRegisterPassword">
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password"
                                      required
                                      name="password"
                                      placeholder="Password"
                                      minLength="6"
                                      onChange={this.onInputChange}
                                      value={this.state.form.password}
                                      disabled={this.state.isLoading}/>
                        <Form.Text className="text-muted">
                            Minimo sei caratteri.
                        </Form.Text>
                    </Form.Group>

                    <Button variant="primary" type="submit" disabled={this.state.isLoading}>
                        <span style={{display: this.state.isLoading ? 'none' : 'block'}}>Registrati</span>
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

export default withRouter(Register)