export interface ILoginCredential {
    emailAddress: string;
    password: string;
}

export interface ILoginResponse {
    message: string;
    jwt: string;
}

export interface IRegisterCredential {
    name: string;
    emailAddress: string;
    password: string;
}