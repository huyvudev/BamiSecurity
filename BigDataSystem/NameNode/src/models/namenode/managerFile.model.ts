import { MetaData } from "../../domain/namenode/metadata.entity";
import mongoose, { Document, Model, Schema } from 'mongoose';
import { paginate, toJSON } from "../plugins";

interface ManagerFileDocument extends Document {
  name: string;
  type: string;
  fileName: string;
}

interface UserModel extends Model<ManagerFileDocument> {
  paginate(filter: Record<string, any>, options: Record<string, any>, searchFields: Record<string, any>): Promise<any>;
}


const managerFileSchema = new Schema<MetaData>({
  name: { type: String, maxLength: 255 },
  type: { type: String, maxLength: 255 },
  fileName: { type: String, maxLength: 255 },
}, {
  timestamps: true,
});

managerFileSchema.plugin(toJSON);
managerFileSchema.plugin(paginate);

const ManagerFile = mongoose.model<ManagerFileDocument, UserModel>('ManagerFile', managerFileSchema);
export default ManagerFile;
