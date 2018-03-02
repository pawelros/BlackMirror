import Credentials from './Credentials';

export default interface User {
    Id: string;
    Name: string;
    Email: string;
    RepositoryCredentials: Credentials[];
}