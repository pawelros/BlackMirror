import SvcRepositoryType from './SvcRepositoryType';

export default interface Credentials {
    Login: string;
    RepositoryType: SvcRepositoryType;
    AllowedRepositories: string[];
    Password: string;
}