import { useVerifyDocument } from '@/api/candidate-api';
import { Button } from '@/components/ui/button';
import type { VerifyCandidateDocumentCommandCorrected } from '@/types/candidate-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import {
    AlertDialog,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from '../ui/alert-dialog';

const verifyCandidateDocumentFormSchema = z.object({
    candidateId: z.string().nonempty('Candidate ID is required'),
    documentId: z.string().nonempty('Document type ID is required'),
}) satisfies z.ZodType<VerifyCandidateDocumentCommandCorrected>;

type Props = {
    candidateId: string;
    documentId: string;
};

export function VerifyDocumentButton({ candidateId, documentId }: Props) {
    const [open, setOpen] = useState(false);
    const verifyCandidateDocMutation = useVerifyDocument();

    const form = useForm<z.infer<typeof verifyCandidateDocumentFormSchema>>({
        resolver: zodResolver(verifyCandidateDocumentFormSchema),
    });

    useEffect(() => {
        form.setValue('candidateId', candidateId);
        form.setValue('documentId', documentId);
    }, [candidateId, documentId]);

    const onSubmit = async (data: VerifyCandidateDocumentCommandCorrected) => {
        verifyCandidateDocMutation.mutate(data, {
            onSuccess: () => {
                form.reset();
                setOpen(false);
            },
        });
    };

    return (
        <AlertDialog open={open} onOpenChange={setOpen}>
            <AlertDialogTrigger asChild>
                <Button variant="outline" type="button">
                    Verify
                </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <form onSubmit={form.handleSubmit(onSubmit)}>
                    <AlertDialogHeader>
                        <AlertDialogTitle>
                            Are you absolutely sure?
                        </AlertDialogTitle>
                        <AlertDialogDescription>
                            You are verifing the candidates document.
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                        <Button
                            type="submit"
                            variant="destructive"
                            disabled={verifyCandidateDocMutation.isPending}
                        >
                            Continue
                        </Button>
                    </AlertDialogFooter>
                </form>
            </AlertDialogContent>
        </AlertDialog>
    );
}
