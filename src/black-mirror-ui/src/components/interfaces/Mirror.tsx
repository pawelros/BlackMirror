import SvcRepository from './SvcRepository';
import User from './User';

export default interface Mirror {
    Id: string;
    Name: string;
    SourceRepository: SvcRepository;
    TargetRepository: SvcRepository;
    TargetRepositoryRefSpec: string;
    Owner: User;
    CreationTime: string;
}