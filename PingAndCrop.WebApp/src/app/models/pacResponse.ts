import { PacRequest } from "./pacRequest";

export interface PacResponse
{
    Url?:string;
    Message?:string;
    CroppedResponse?: string;
    RawResponse?: string;
    Error?: string;
}
