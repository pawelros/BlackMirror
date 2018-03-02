export default interface NewUser {
    Id: string;
    Name: string;
    Email: string;
    repositoryCredentials: {
        newLogin: string;
        newType: string;
        newPassword: string;
        existing: Array<any>;
        add(): void;

        delete(c: any): void;
    };
    status: string;
    save(): void;
}
