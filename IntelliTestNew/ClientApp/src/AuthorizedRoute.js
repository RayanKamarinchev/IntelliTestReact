﻿import React, { useContext } from 'react';
import { Route } from 'react-router';
import UserContext from './auth/user';

const AuthorizedRoute = props => {

    const { isForAuthenticated ,children, ...otherProps } = props;

    const { userConfig } = useContext(UserContext);

    return (
        <>
            {(userConfig.signedIn === isForAuthenticated) && (
                <Route {...otherProps}>
                    {children}
                </Route>
            )}
        </>
    );
};

export default AuthorizedRoute;