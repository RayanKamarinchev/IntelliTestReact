import React from 'react';
import * as Mng from './';
import AuthorizedRoute from '../../../AuthorizedRoute';

const Manage = () => {

    return (
        <Mng.ManageLayout>
            <AuthorizedRoute exact path='/Account/Manage' component={Mng.ManageIndex} />
            <AuthorizedRoute exact path='/Account/Manage/ChangePassword' component={Mng.ChangePassword} />
            <AuthorizedRoute exact path='/Account/Manage/Email' component={Mng.Email} />
            <AuthorizedRoute exact path='/Account/Manage/ExternalLogins' component={Mng.ExternalLogins} />
            <AuthorizedRoute exact path='/Account/Manage/PersonalData' component={Mng.PersonalData} />
            <AuthorizedRoute exact path='/Account/Manage/SetPassword' component={Mng.SetPassword} />
        </Mng.ManageLayout>
    );
};

export default Manage;