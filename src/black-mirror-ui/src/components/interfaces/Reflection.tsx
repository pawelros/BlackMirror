import Mirror from './Mirror';
import Synchronization from './Synchronization';
import Revision from './Revision';

export default interface Reflection {
    Id: string;
    DateTime: string;
    Mirror: Mirror;
    Synchronization: Synchronization;
    SourceRevision: Revision;
    TargetRevision: Revision;
}