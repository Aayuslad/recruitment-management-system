import { useGetDocumentTypes } from '@/api/document-api';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectLabel,
    SelectTrigger,
    SelectValue,
} from '@/components/ui/select';

type Props = {
    selectedTypeId: string;
    setSelectedTypeId: (id: string) => void;
};

export function DocumentTypeSelector({
    selectedTypeId,
    setSelectedTypeId,
}: Props) {
    const { data: docTypes } = useGetDocumentTypes();

    return (
        <Select
            value={selectedTypeId}
            onValueChange={(value) => setSelectedTypeId(value)}
        >
            <SelectTrigger className="w-[250px]">
                <SelectValue placeholder="Select document type" />
            </SelectTrigger>
            <SelectContent>
                <SelectGroup>
                    <SelectLabel>Documents</SelectLabel>
                    {docTypes?.map((docType) => (
                        <SelectItem key={docType.id} value={docType.id}>
                            {docType.name}
                        </SelectItem>
                    ))}
                </SelectGroup>
            </SelectContent>
        </Select>
    );
}
