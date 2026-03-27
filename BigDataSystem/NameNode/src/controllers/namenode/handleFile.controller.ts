import dotenv from 'dotenv';
import path from 'path';
import ManagerDatanode from '../../models/namenode/managerDatanode.model';
import MetaData from '../../models/namenode/matadata.model';
import httpStatus from 'http-status';
import ManagerFile from '../../models/namenode/managerFile.model';
import catchAsync from '../../utils/catchAsync';
import pick from '../../utils/pick';

dotenv.config({ path: path.join(__dirname, '../.env') })

const checkUploadFile = async (req: any, res: any) => {
  const dataBody = req.body;
  console.log(dataBody)
  let numberChunk;

  const size = Number(dataBody?.size);
  const MB150 = Number(process.env.MB150);
  const MB200 = Number(process.env.MB200);

  if (size < MB150) {
    numberChunk = 3;
  }
  else if (size >= MB150 && size <= MB200) {
    numberChunk = 4;
  }
  else {
    numberChunk = 5;
  }

  await ManagerDatanode.findOne({ namenodeId: "123qwe" })
    .then(async (data: any) => {
      const datanodeAlive = [];
      const datanodeWrite = [];
      const datanodeReplication1 = []
      const datanodeReplication2 = []
      const metaDatas = []
      const managerFiles = []

      managerFiles.push({
        name: dataBody?.name,
        type: dataBody?.type,
        fileName: dataBody?.fileName,
      });

      for (let i = 1; i < 5; i++) {
        if (data[`datanode${i}`]?.alive) {
          datanodeAlive.push(data[`datanode${i}`])
        }
      }

      if (datanodeAlive.length >= 3) {
        for (let i = 0; i < numberChunk; i++) {
          const datanodeSave = datanodeAlive.slice()
          datanodeWrite.push(getRandom(datanodeSave));
          datanodeReplication1.push(getRandom(datanodeSave));
          datanodeReplication2.push(getRandom(datanodeSave));
        }
      } else if (datanodeAlive.length == 2) {
        for (let i = 0; i < numberChunk; i++) {
          const random = Math.floor(Math.random() * datanodeAlive.length)
          datanodeWrite.push(datanodeAlive[0]);
          datanodeReplication1.push(datanodeAlive[1]);
          datanodeReplication2.push(datanodeAlive[random]);
        }
      } else {
        for (let i = 0; i < numberChunk; i++) {
          datanodeWrite.push((datanodeAlive[0]));
          datanodeReplication1.push(datanodeAlive[0]);
          datanodeReplication2.push(datanodeAlive[0]);
        }
      }

      for (let i = 0; i < numberChunk; i++) {
        metaDatas.push({
          name: dataBody?.name,
          type: dataBody?.type,
          fileName: dataBody?.fileName,
          index: i + 1,
          datanode: datanodeWrite[i]?.address,
          datanodeReplication1: datanodeReplication1[i]?.address,
          datanodeReplication2: datanodeReplication2[i]?.address,
        })
      }
      await ManagerFile.insertMany(managerFiles)
      await MetaData.insertMany(metaDatas)
        .then(
          res.send({
            metaDatas: metaDatas,
            numberChunk: numberChunk,
          })
        )
    })

}

function getRandom(arr: any) {
  const randomIndex = Math.floor(Math.random() * arr.length);
  const randomElement = arr[randomIndex];
  arr.splice(randomIndex, 1);
  return randomElement;
}

const checkReadFile = async (req: any, res: any) => {
  const name = req.query.name;
  console.log(name)
  await MetaData.find({ name: name })
    .then(async (data: any) => {
      console.log("file : ", data)
      if (data.length == 0) {
        res.status(httpStatus.NOT_FOUND).send();
      }
      else {
        const metaDatas: any[] = [];
        await ManagerDatanode.findOne({ namenodeId: "123qwe" })
          .then(async (managerData: any) => {
            const datanodeAlive: any[] = [];
            for (let i = 1; i < 5; i++) {
              if (managerData[`datanode${i}`]?.alive) {
                datanodeAlive.push(managerData[`datanode${i}`].address)
              }
            }
            // console.log(datanodeAlive)

            data.forEach((e: { datanode: any; name: any; index: any; type: any; datanodeReplication1: any; datanodeReplication2: any; }) => {
              // console.log(e)
              if (datanodeAlive.includes(e.datanode)) {
                metaDatas.push({
                  name: e?.name,
                  index: e?.index,
                  datanode: e?.datanode,
                  type: e?.type
                })
                return;
              }
              else if (datanodeAlive.includes(e?.datanodeReplication1)) {
                metaDatas.push({
                  name: e?.name,
                  index: e?.index,
                  datanode: e?.datanodeReplication1,
                  type: e?.type
                })
                return;
              }
              else if (datanodeAlive.includes(e?.datanodeReplication2)) {
                metaDatas.push({
                  name: e?.name,
                  index: e?.index,
                  datanode: e?.datanodeReplication2,
                  type: e?.type
                })
                return;
              }
            });
          })
        res.send({
          metadata: metaDatas,
        })
      }
    })
}
const getAllFile = catchAsync(async (req: any, res: any) => {
  const filter = pick(req.query, ['name', 'role']);
  const options = pick(req.query, ['sortBy', 'limit', 'page']);
  const searchFields = pick(req.query, ['name']);
  console.log("options: ", req.query)
  const result = await ManagerFile.paginate(filter, options, searchFields)
  res.send(result);
});
export default {
  checkUploadFile,
  checkReadFile,
  getAllFile
}

