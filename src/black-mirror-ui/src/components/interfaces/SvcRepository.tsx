import SvcRepositoryType from './SvcRepositoryType';
import User from './User';
import Credentials from './Credentials';

export default interface SvcRepository {
    Id: string;
    Type: SvcRepositoryType;
    Name: string;
    Uri: string;
    DefaultCommitMessagePrefix: string;
    CheckoutUser: User;
    PushUser: User;
    MappedCheckoutCredentials: Credentials;
}