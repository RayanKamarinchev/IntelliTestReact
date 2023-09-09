import Api from '../../api';

export const useAccount = () => {
        
    const GetExternalLogins = async () => {

        return new Promise((resolve, reject) => {

            Api.get('Account/ExternalLogins')
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));

        });
    };

    const HasPassword = async () => {
        return new Promise((resolve, reject) => {

            Api.post('Account/HasPassword', {})
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));

        });
    };

    const Register = async (registerObj) => {
        return new Promise((resolve, reject) => {
            Api.get(`Account/Registers?email=${registerObj.email}&password=${registerObj.password}&confirmPassword=${registerObj.confirmPassword}&returnUrl=${registerObj.returnUrl}`, registerObj)
                .then(resp => {     
                    resolve(resp.data);
                })
                .catch(err => reject(err));

        });
    };

    const Login = async (loginObj) => {

        return new Promise((resolve, reject) => {
            //console.log(loginObj);
            Api.post('Account/Login', loginObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const Logout = async (returnUrl) => {

        return new Promise((resolve, reject) => {

            Api.post('Account/Logout', { returnUrl: returnUrl })
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const UpdateProfile = async (profileObj) => {

        return new Promise((resolve, reject) => {

            Api.post('Account/ManageProfile', profileObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };



    const GetSignedInUser = async () => {
        return new Promise((resolve, reject) => {
            Api.post('Account/SignedInUser', {})
                .then(resp => {
                    //console.log(resp.data);
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const IsEmailConfirmed = async () => {
        return new Promise((resolve, reject) => {
            Api.post('Account/EmailConfirmed', {})
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const SendEmailVerification = async () => {
        return new Promise((resolve, reject) => {
            Api.post('Account/SendEmailVerification', {})
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const ChangeEmail = async (emailObj) => {
        return new Promise((resolve, reject) => {
            Api.post('Account/ChangeEmail', emailObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    }

    const ChangePassword = async (pwObj) => {
        return new Promise((resolve, reject) => {
            Api.post('Account/ChangePassword', pwObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const SetPassword = async (pwObj) => {
        return new Promise((resolve, reject) => {
            Api.post('Account/SetPassword', pwObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const ForgotPassword = async (emailObj) => {
        return new Promise((resolve, reject) => {
            Api.post('Account/ForgotPassword', emailObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const ResendEmailConfirmation = async (emailObj) => {
        return new Promise((resolve, reject) => {
            Api.post('Account/ResendEmailConfirmation', emailObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const ResetPassword = async (resetObj) => {
        return new Promise((resolve, reject) => {
            Api.post('Account/ResetPassword', resetObj)
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const IsMachineRemembered = async () => {
        return new Promise((resolve, reject) => {
            Api.get('Account/IsMachineRemembered')
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const ResetAuthenticator = async () => {
        return new Promise((resolve, reject) => {
            Api.post('Account/ResetAuthenticator', {})
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    };

    const GetUserPhotoPath = async () => {
        return new Promise((resolve, reject) => {
            Api.get('Account/GetUserPhotoPath', {})
                .then(resp => {
                    resolve(resp.data);
                })
                .catch(err => reject(err));
        });
    }

    return {
        GetExternalLogins, HasPassword, Register, Login,
        Logout, UpdateProfile, GetSignedInUser, IsEmailConfirmed,
        SendEmailVerification, ChangeEmail, ChangePassword, SetPassword,
        ForgotPassword,
        ResendEmailConfirmation, ResetPassword, IsMachineRemembered, ResetAuthenticator
    };
};