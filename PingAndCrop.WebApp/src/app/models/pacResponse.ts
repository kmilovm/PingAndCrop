import { PacRequest } from "./pacRequest";

export interface PacResponse
{
    Error?: string;
    RawResponse?: string;
    CroppedResponse?: string;
    MessageId?:string;
    PacRequest: PacRequest;
}
