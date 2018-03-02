export default interface NewMirror {
    Name: string;
    SourceRepositoryId: string;
    TargetRepositoryId: string;
    status: string;
    error: any;
    save(payload: any): void;
}