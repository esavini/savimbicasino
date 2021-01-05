import React from 'react'

const DefaultValue = {
    isLogged: false
    , user: {
        token: null
    }
    ,login: (token, callBack) => {
        this.isLogged = true
        this.user.token = token
        
        callBack()
    }
}

const UserContext = React.createContext(DefaultValue)

export const UserProvider = UserContext.Provider
export const UserConsumer = UserContext.Consumer

export default UserContext